using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using Newtonsoft.Json;
using SimulatorInterface;

public class FlowManager : CPEventListener
{
	[Header("-- Calling events --")]
	public GameEvent modelChangedEvent;
	public GameEvent connectionChangedEvent;
	public GameEvent screenFadeEvent;
	public GameEvent showStandardDialogEvent;
	public GameEvent scanForNewModelEvent;
	public GameEvent screenfadeEvent;

	[Header("-- Properties --")]
	public ComponentSetup[] componentSetups;

	private ComponentSetup currentLoadedSetup = null;

	// New Image scanned
	public override void OnEventRaised(object val)
	{
		
		screenFadeEvent?.Raise(true);

		var imageName = (string)val;
		Debug.Log("Image found: " + imageName);
		var setup = componentSetups.FirstOrDefault(e => e.imageName == imageName);
		if (setup != null)
		{
			// Get toolbox if server connection changed
			StartCoroutine(GetSettingsFromServer(setup));

			//connectionChangedEvent?.Raise(mapping.setup);
//			getSettingsFromServerEvent?.Raise(setup);

			currentLoadedSetup = setup;
		}
		else
			Debug.LogError("FlowManager: Setup not found, name: " + imageName);
	}

	private IEnumerator GetToolBoxFromServer(object s)
	{
		ComponentSetup setup = s as ComponentSetup;

		string username = "dM3psBjrX5ZXxegEuJfkYtkq";
		string password = "neyhSHrPUnNs8EucwTStkvqb";
		string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

		UnityWebRequest www = UnityWebRequest.Get("http://" + CPSettings.Instance.GetConfig().serverIp + ":" + setup.port + "/tools");
		www.SetRequestHeader("Accept", "application/json");
		www.SetRequestHeader("Authorization", "Basic " + encoded);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			ComponentManager.Instance.SetCompleteToolbox(www.downloadHandler.text);
			
			Helper.FireEventInMainThread(modelChangedEvent, setup);
			Helper.FireEventInMainThread(connectionChangedEvent, setup);
		}
	}

	private IEnumerator GetSettingsFromServer(object s)
	{
		ComponentSetup setup = s as ComponentSetup;

		string username = "dM3psBjrX5ZXxegEuJfkYtkq";
		string password = "neyhSHrPUnNs8EucwTStkvqb";
		string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

		UnityWebRequest www = UnityWebRequest.Get("http://" + CPSettings.Instance.GetConfig().serverIp + ":" + setup.port + "/settings");
		www.SetRequestHeader("Accept", "application/json");
		www.SetRequestHeader("Authorization", "Basic " + encoded);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			// Check version
			ServerSettings settings = JsonConvert.DeserializeObject<ServerSettings>(www.downloadHandler.text);
			string[] serverVersion = settings.version.Split('.');
			string[] clientVersion = Application.version.Split('.');

			if (!(serverVersion[0] == clientVersion[0] && serverVersion[1] == clientVersion[1]))
			{
				// Show error message
				showStandardDialogEvent?.Raise(new DialogSpawnInfo(10001));
				Helper.FireEventInMainThread(scanForNewModelEvent, null);
				Helper.FireEventInMainThread(screenfadeEvent, false);
			}
			else
			{
				StartCoroutine(GetToolBoxFromServer(setup));
			}
		}
	}
}
