using Cadpeople.Events;
using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ToolSelector : MonoBehaviour
{
	public GameEvent logEvent;
	public GameEvent clickedOnToolEvent;
	public string toolName;
	
	public void Clicked()
	{
		logEvent?.Raise("Tool " + toolName + " clicked");
		clickedOnToolEvent?.Raise(toolName);
	}
}

