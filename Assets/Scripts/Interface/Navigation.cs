using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {

	public GameEvent unloadScenarioEvent;
	public GameEvent arTrackedImageEvent;

	public void CloseApplication()
	{
		Application.Quit();
	}

	public void Home()
	{
		unloadScenarioEvent?.Raise("");
	}

	public void ARTrackedImage()
	{
		arTrackedImageEvent?.Raise("group-1");
	}
}
