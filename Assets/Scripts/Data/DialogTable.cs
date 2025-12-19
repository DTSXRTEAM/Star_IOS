using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class DialogTableEntry
{
	public int Id;
	public string HeaderText;
	public string BodyText;
	public string ImageUrl;
	public string[] Buttons;
	public string Type;
	public string[] Choices;
}

public class DialogTable : Singleton<DialogTable> {

	private DialogTableEntry[] Entries;

	private void Awake()
	{
		Entries = null;
		StartCoroutine(LoadLocalConfig("DialogTable", OnConfigLoaded));
	}

	public IEnumerator LoadLocalConfig(string configFileName, Action<DialogTableEntry[]> loadCallback)
	{
		string uri = "file://" + Application.streamingAssetsPath + "/" + configFileName + ".json";

		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();

			string[] pages = uri.Split('/');
			int page = pages.Length - 1;

			if(webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
			else
			{
				try
				{
					DialogTableEntry[] entries = JsonConvert.DeserializeObject<DialogTableEntry[]>(webRequest.downloadHandler.text);
					loadCallback(entries);
				}
				catch(Exception ex)
				{
					Debug.Log("Error loading DialogTable.json, ex: " + ex.Message);
					throw;
				}
			}
		}
	}
	
	public DialogTableEntry GetTableEntry(int id)
	{
		var entry = Entries.FirstOrDefault(e => e.Id == id);
		if(entry == null)
		{
			entry = new DialogTableEntry
			{
				HeaderText = "Not found",
				BodyText = "Errorcode: " + id + " not found in table",
				Buttons = new string[] { "OK" }
			};
		}
		return entry;
	}
	 
	private void OnConfigLoaded(DialogTableEntry[] localConfig)
	{
		Entries = localConfig;
	}
}
