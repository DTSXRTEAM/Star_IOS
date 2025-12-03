using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EventDebugListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		//var gameEvents = GetAssetList<GameEvent>("Events");

		//// Parse folders
		//string[] folderEntries = Directory.GetDirectories(Application.dataPath + "/Events");
		//foreach (string folderName in folderEntries)
		//{
		//	var res = GetAssetList<GameEvent>("Events/" + Path.GetFileName(folderName));
		//	gameEvents.AddRange(res);
		//}

		//// Add list of gameEventListeners and hook up
	       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//public static List<T> GetAssetList<T>(string path) where T : class
	//{
	//	string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

	//	return fileEntries.Select(fileName =>
	//	{
	//		string temp = fileName.Replace("\\", "/");
	//		int index = temp.LastIndexOf("/");
	//		string localPath = "Assets/" + path;

	//		if (index > 0)
	//			localPath += temp.Substring(index);

	//		return AssetDatabase.LoadAssetAtPath(localPath, typeof(T));
	//	})
	//		//Filtering null values, the Where statement does not work for all types T
	//		.OfType<T>()    //.Where(asset => asset != null)
	//		.ToList();
	//}
}
