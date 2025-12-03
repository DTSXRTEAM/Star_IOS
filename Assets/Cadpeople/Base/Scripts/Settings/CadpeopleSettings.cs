using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Cadpeople.Base
{
#if UNITY_EDITOR
    [CustomEditor(typeof(CadpeopleSettings))]
    public class CadpeopleSettingsEditor : Editor
    {

        private CadpeopleSettings myScript;
        private bool showLogging;
        private bool showStatistics;

        private void OnEnable()
        {
            myScript = (CadpeopleSettings)target;
        }

        public override void OnInspectorGUI()
        {
            //-----------------------------------LOGGING---------------------------------\\
            GUILayout.BeginVertical("Box");
            showLogging = EditorGUILayout.Foldout(showLogging, "Logging");

            if (showLogging)
            {
                myScript.logToFile = EditorGUILayout.Toggle("Enable logging", myScript.logToFile);
                if (myScript.logToFile)
                {
                    myScript.logFilePath = EditorGUILayout.TextField("Log file path", myScript.logFilePath);
                }
            }

            GUILayout.EndVertical();
            //-----------------------------------STATISTICS---------------------------------\\
            GUILayout.BeginVertical("Box");
            showStatistics = EditorGUILayout.Foldout(showStatistics, "Statistics");

            if (showStatistics)
            {

                myScript.logStatistics = EditorGUILayout.Toggle("Enable statistics", myScript.logStatistics);

                if (myScript.logStatistics)
                {
                    myScript.statisticsLogger = (Statistics.IStatisticsLogger)EditorGUILayout.ObjectField("Statistics logger", myScript.statisticsLogger, typeof(Statistics.IStatisticsLogger), false);
                    myScript.statisticsFilePath = EditorGUILayout.TextField("Statistics file path", myScript.statisticsFilePath);
                    myScript.sendDataToBackend = EditorGUILayout.Toggle("Send data to backend", myScript.sendDataToBackend);

                    if (myScript.sendDataToBackend)
                    {
                        myScript.backendUrl = EditorGUILayout.TextField("Backend url", myScript.backendUrl);
                        myScript.continousDataSendDelay = EditorGUILayout.FloatField("Send data every (seconds)", myScript.continousDataSendDelay);
                    }

                }
            }

            

            GUILayout.EndVertical();

            EditorUtility.SetDirty(myScript);
        }
    }
#endif

    [CreateAssetMenu(menuName = "Cadpeople/Settings")]
    public class CadpeopleSettings : ScriptableObject
    {
        //-----------------------------------LOGGING---------------------------------\\
        public bool logToFile;
        public string logFilePath;
        //-----------------------------------STATISTICS---------------------------------\\
        public bool logStatistics;
        public Statistics.IStatisticsLogger statisticsLogger;
        public string statisticsFilePath;
        public bool sendDataToBackend;
        public string backendUrl;
        public float continousDataSendDelay;
    }
}


