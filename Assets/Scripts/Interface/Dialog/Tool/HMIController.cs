using Cadpeople.Events;
using Data;
using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HMIController : ToolDialogController
{
	public GameObject WebView = null;

	private UniWebView uniWebViewScript;
	private string HMIUrl = "";

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();
		if (WebView != null)
		{
			uniWebViewScript = WebView.GetComponent<UniWebView>();

			var HMIUrl = NetworkManager.Instance.ServerUrl + "/plugins/service-panel";
			uniWebViewScript.urlOnStart = HMIUrl;
		}
	}

	public override void OnEventRaised(object par)
	{
		// Close if tool is changed
		if (par.GetType() == typeof(SimulatorInterface.Toolbox))
		{
			var toolbox = (SimulatorInterface.Toolbox)par;

			if (toolbox.name.ToLower() != ToolName.ToLower())
			{
				Destroy(gameObject);
			}
		}
	}

}
