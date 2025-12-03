using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubInSafeModeDialogController : ToolDialogController
{
	private GameObject EnterSafeModeButton;
	private GameObject LeaveSafeModeButton;

	protected override void Awake()
	{
		base.Awake();

		EnterSafeModeButton = contentArea.Find("EnterSafeModeButton").gameObject;
		LeaveSafeModeButton = contentArea.Find("LeaveSafeModeButton").gameObject;

		var toggle = ComponentManager.Instance.GetStoredComponent(ComponentName);
		if (toggle != null)
		{
			UpdateDialog(((SimulatorInterface.Toggle)toggle).enabled);
		}
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var toggle = (SimulatorInterface.Toggle)par;

			if (toggle.name.ToLower() == ComponentName.ToLower())
			{
				UpdateDialog(toggle.enabled);
			}
		}

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

	protected void UpdateDialog(bool safemode)
	{
		if(safemode)
		{
			EnterSafeModeButton.SetActive(false);
			LeaveSafeModeButton.SetActive(true);
		}
		else
		{
			EnterSafeModeButton.SetActive(true);
			LeaveSafeModeButton.SetActive(false);
		}
	}

	public void EnterSaveMode()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { enabled = true } });

	}

	public void LeaveSafeMode()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { enabled = false } });
	}
}
