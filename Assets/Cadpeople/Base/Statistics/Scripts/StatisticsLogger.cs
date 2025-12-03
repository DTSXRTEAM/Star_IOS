using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cadpeople.Base;
using System.IO;
using Base_data_models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using UnityEngine.Networking;
using System;

namespace Cadpeople.Base.Statistics
{

    public abstract class IStatisticsLogger : ScriptableObject
    {
        /// <summary>
        /// Logs a statistics types to the statisticslogger. Count is how much the value of the statisticstype
        /// should increment.
        /// </summary>
        /// <param name="type">The statisticstype to log</param>
        /// <param name="count">How much the value should increment</param>
        public abstract void Log(StatisticsType type, float count);
        /// <summary>
        /// Log a objectviewlog object. Used by the heatmap generation.
        /// </summary>
        /// <param name="viewData">An object representing how long an object has been viewed</param>
        public abstract void LogObjectInView(StatisticsObjectViewLog viewData);

    }

    [CreateAssetMenu(menuName = "Cadpeople/Statistics logger")]
    public class StatisticsLogger : IStatisticsLogger
    {

        //-----------------------------------EDITOR VARIABLES---------------------------------\\
        [SerializeField]
        private CadpeopleSettings cadpeopleSettings;
        //-----------------------------------PRIVATE VARIABLES---------------------------------\\
        private Dictionary<string, StatisticsProperty> statisticsPropertiesDict;
        private List<StatisticsObjectViewLog> objectViewLogs;
        private StatisticsSession currentSession;
        private string statisticsPath;
        private string backendUrl;
        private float continousSendDelay;

        //--------------------------------------------------------------------------------\\
        //									PUBLIC METHODS
        //--------------------------------------------------------------------------------\\

        /// <summary>
        /// Initializes the statisticslogger by creating lists, dictionaries etc. It also creates the necessary folders
        /// and sends the data to the backend if needed.
        /// </summary>
        /// <param name="caller">You should provide a monobehaviour to be able to start coroutine</param>
        public void Init(MonoBehaviour caller)
        {
            // Initialize lists
            statisticsPropertiesDict = new Dictionary<string, StatisticsProperty>();
            objectViewLogs = new List<StatisticsObjectViewLog>(); 
            // Create and set current session
            currentSession = new StatisticsSession
            {
                deviceId = SystemInfo.deviceUniqueIdentifier,
                sessionStartTime = System.DateTime.Now,
                id = Guid.NewGuid()
            };
            // Set variables based on cadpeople settings
            statisticsPath = cadpeopleSettings.statisticsFilePath + "\\Statistics";
            backendUrl = cadpeopleSettings.backendUrl;
            continousSendDelay = cadpeopleSettings.continousDataSendDelay;
            // Create statistics folder if it does not exist
            if (!Directory.Exists(statisticsPath))
                Directory.CreateDirectory(statisticsPath);
            // Check if there are any unsend cached statistics data that needs to be send to the server
            foreach (var statisticsFile in Directory.EnumerateFiles(statisticsPath))
            {
                try
                {
                    var cachedStatisticsData = JsonConvert.DeserializeObject<StatisticsSessionData>(File.ReadAllText(statisticsFile));
                    if (cadpeopleSettings.sendDataToBackend)
                    {
                        caller.StartCoroutine(PostDataToServer(cachedStatisticsData, backendUrl + "//umbraco/api/myapi/sendstatisticsdata",
                            () =>
                            {
                                // Success delete cached file
                                File.Delete(statisticsFile);
                            }, () => { }));
                    }
                    
                } catch(Exception e)
                {
                    Debug.LogError("Could not deserialize cached statistics data. Exception: " + e.Message);
                }
                
            }
            // Start continous send statistics to server
            if(cadpeopleSettings.sendDataToBackend)
                caller.StartCoroutine(ContinuousPostToServer(caller, backendUrl + "//umbraco/api/myapi/sendstatisticsdata", continousSendDelay));
        }
        /// <summary>
        /// Call this when the program is exited. Will write statistics data to a file.
        /// </summary>
        public void OnExitProgram()
        {
            // Set session duration
            currentSession.sessionDuration = Time.time;
            
            // Write statistics to file after the program ends
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                WriteStatisticsToFile(statisticsPath);
            });
        }
        /// <summary>
        /// Logs a statistics types to the statisticslogger. Count is how much the value of the statisticstype
        /// should increment.
        /// </summary>
        /// <param name="type">The statisticstype to log</param>
        /// <param name="count">How much the value should increment</param>
        public override void Log(StatisticsType type, float count)
        {
            CheckAndAddStatisticsToDict(type);

            statisticsPropertiesDict[type.name].value += count;
        }
        /// <summary>
        /// Log a objectviewlog object. Used by the heatmap generation.
        /// </summary>
        /// <param name="viewData">An object representing how long an object has been viewed</param>
        public override void LogObjectInView(StatisticsObjectViewLog viewData)
        {
            objectViewLogs.Add(viewData);
        }

        //--------------------------------------------------------------------------------\\
        //									PRIVATE METHODS
        //--------------------------------------------------------------------------------\\

        /// <summary>
        /// Writes all data to a local file. Remember this method will not delete stored data in the statisticslogger.
        /// </summary>
        /// <param name="path">Path of the file without filename and extention</param>
        private void WriteStatisticsToFile(string path)
        {
            var fullPath = path + "\\" + System.DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");

            // Write data to file
            using (var streamWriter = new StreamWriter(fullPath + ".json"))
            {
                var json = JsonConvert.SerializeObject(new StatisticsSessionData
                {
                    session = currentSession,
                    objectviewLogs = objectViewLogs,
                    properties = statisticsPropertiesDict.Values.ToList()
                });
                streamWriter.WriteLine(json);
            }
        }
        /// <summary>
        /// Check if statisticstype is in the dictionary, if not it will be added.
        /// </summary>
        /// <param name="statisticsType">The statisticstype to add</param>
        private void CheckAndAddStatisticsToDict(StatisticsType statisticsType)
        {
            if (!statisticsPropertiesDict.ContainsKey(statisticsType.name))
            {
                statisticsPropertiesDict.Add(statisticsType.name, 
                    new StatisticsProperty
                    {
                        name = statisticsType.name,
                        calculateAverage = statisticsType.calculateAverage
                    });
            }
        }

        /// <summary>
        /// Continous try to post the statistics data to the backend. Its recommended to set updateRate to at least 10 seconds.
        /// </summary>
        /// <param name="caller">A monobehaviour is needed to start coroutines.</param>
        /// <param name="url">The url to post the data to.</param>
        /// <param name="updateRate">How often it should try to post the data to the server in seconds.</param>
        /// <returns></returns>
        private IEnumerator ContinuousPostToServer(MonoBehaviour caller, string url, float updateRate)
        {

            while (true)
            {
                yield return new WaitForSeconds(updateRate);

                var objectViewLogsToBeSend = objectViewLogs.GetRange(0, Math.Min(100, objectViewLogs.Count));
                objectViewLogs.RemoveRange(0, Math.Min(100, objectViewLogs.Count));
                var propertiesToBeSend = statisticsPropertiesDict.Values.ToList();
                statisticsPropertiesDict.Clear();
                // Update session duration
                currentSession.sessionDuration = Time.time;
                // Create session data
                var statisticsSessionData = new StatisticsSessionData
                {
                    objectviewLogs = objectViewLogsToBeSend,
                    properties = propertiesToBeSend,
                    session = currentSession
                };
                // Post to server
                caller.StartCoroutine(PostDataToServer(statisticsSessionData, url,
                    () =>
                    {
                        // Success
                    },
                    () =>
                    {
                        // Error: Readd all the data to be able to save it to a file
                        objectViewLogs.AddRange(objectViewLogsToBeSend);
                        for (int i = 0; i < propertiesToBeSend.Count; i++)
                        {
                            Log(new StatisticsType { name = propertiesToBeSend[i].name }, propertiesToBeSend[i].value);
                        }
                    }));
            }

        }

        /// <summary>
        /// Posts the statisticssessiondata to the backend server.
        /// </summary>
        /// <param name="sessionData">The data to send</param>
        /// <param name="url">Where to send the data</param>
        /// <param name="SuccessCallback">Called on success</param>
        /// <param name="ErrorCallback">Called on error</param>
        /// <returns></returns>
        private IEnumerator PostDataToServer(StatisticsSessionData sessionData, string url, Action SuccessCallback, Action ErrorCallback)
        {
            // Convert to json
            var json = JsonConvert.SerializeObject(sessionData);
            // Create request
            var webRequest = new UnityWebRequest(url, "POST");
            var bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Send
            yield return webRequest.SendWebRequest();

            // Handle error or success
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log("Error sending statistics to server on " + url + ". Message: " + webRequest.downloadHandler.text);
                ErrorCallback();
            }
            else
            {
                Debug.Log("Successfully posted statistics to server " + url);
                SuccessCallback();
            }
        }
    }

}

