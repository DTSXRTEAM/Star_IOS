using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDialogTool : MonoBehaviour
{
	[SerializeField] public GameEvent sendOjectToServerEvent;

	public void OnClose()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = "toolbox", data = new { tool = "pointer" } });
	}
}
