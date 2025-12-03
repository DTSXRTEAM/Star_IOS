using Cadpeople.Events;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ToolDialogController : CPEventListener {

	[SerializeField] protected GameEvent sendOjectToServerEvent;
	[SerializeField] protected string ComponentName;
	[SerializeField] protected string ToolName;

	protected Transform contentArea;
	protected RectTransform contentAreaRect;

	// Use this for initialization
	protected virtual void Awake ()
	{
		contentArea = transform.Find("ContentArea");
		contentAreaRect = contentArea.GetComponent<RectTransform>();
	}

	//public override void OnEventRaised(object par)
	//{
	//	if (par.GetType() == typeof(SimulatorInterface.Enumerator))
	//	{
	//		var enumerator = (SimulatorInterface.Enumerator)par;

	//		if (enumerator.name.ToLower() == ComponentName.ToLower())
	//		{
	//			UpdateDialog(enumerator.index);
	//		}
	//	}

	//	// Close if tool is changed
	//	if (par.GetType() == typeof(SimulatorInterface.Toolbox))
	//	{
	//		var toolbox = (SimulatorInterface.Toolbox)par;

	//		if (toolbox.name.ToLower() != "hmi-tool")
	//		{
	//			Destroy(gameObject);
	//		}
	//	}
	//}
}
