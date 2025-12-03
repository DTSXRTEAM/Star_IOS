using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PumpDialogController :ToolDialogController
{
	[SerializeField] private string EnumeratorComponentName;

	private GameObject StartPumpButton;
	private GameObject StopPumpButton;
	private GameObject StartPumpButtonDisabled;
	private GameObject HoseConnectText;
	private GameObject HoseConnectedText;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();

		StartPumpButton = contentArea.Find("StartPumpButton").gameObject;
		StopPumpButton = contentArea.Find("StopPumpButton").gameObject;
		StartPumpButtonDisabled = contentArea.Find("StartPumpButtonDisabled").gameObject;
		HoseConnectText = contentArea.Find("HoseConnectText").gameObject;
		HoseConnectedText = contentArea.Find("HoseConnectedText").gameObject;

		var pumpstartedToggle = ComponentManager.Instance.GetStoredComponent(ComponentName);
		var pumpEnabledEnumerator = ComponentManager.Instance.GetStoredComponent(EnumeratorComponentName);
		if (pumpstartedToggle != null && pumpEnabledEnumerator != null)
		{
			UpdateDialog(((SimulatorInterface.Enumerator)pumpEnabledEnumerator).index, ((SimulatorInterface.Toggle)pumpstartedToggle).enabled);
		}
	}

	public override void OnEventRaised(object par)
	{
		// Pump start/stop
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var toggle = (SimulatorInterface.Toggle)par;

			if (toggle.name.ToLower() == ComponentName.ToLower())
			{
				var pumpEnabledEnumerator = ComponentManager.Instance.GetStoredComponent(EnumeratorComponentName);
				if (pumpEnabledEnumerator != null)
				{
					UpdateDialog(((SimulatorInterface.Enumerator)pumpEnabledEnumerator).index, toggle.enabled);
				}
			}
		}

		// Pump enable
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var enumerator = (SimulatorInterface.Enumerator)par;

			if (enumerator.name.ToLower() == EnumeratorComponentName.ToLower())
			{
				var pumpstartedToggle = ComponentManager.Instance.GetStoredComponent(ComponentName);
				if (pumpstartedToggle != null)
				{
					UpdateDialog(enumerator.index, ((SimulatorInterface.Toggle)pumpstartedToggle).enabled);
				}
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


	protected void UpdateDialog(int index, bool started)
	{
		if (index == 2)
		{
			StartPumpButtonDisabled.SetActive(false);
			HoseConnectedText.SetActive(true);
			HoseConnectText.SetActive(false);
			if (started)
			{
				StartPumpButton.SetActive(false);
				StopPumpButton.SetActive(true);
			}
			else
			{
				StartPumpButton.SetActive(true);
				StopPumpButton.SetActive(false);
			}
		}
		else
		{
			HoseConnectText.SetActive(true);
			HoseConnectedText.SetActive(false);
			StartPumpButton.SetActive(false);
			StopPumpButton.SetActive(false);
			StartPumpButtonDisabled.SetActive(true);
		}
	}

	public void OnPumpOn()
	{
		//sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 2 } });
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = "pump", data = new { enabled = true } });

	}

	public void OnPumpOff()
	{
		//sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 1 } });
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = "pump", data = new { enabled = false } });
	}

}
