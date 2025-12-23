using Cadpeople.Events;
using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ComponentInteractable : Interactable
{
	public override void Click()
	{
		//logEvent?.Raise(name + " was clicked");
		buttonClickedEvent?.Raise(name);
		Debug.Log(name + " was clicked");
    }
}

