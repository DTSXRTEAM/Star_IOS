using Cadpeople.Events;
using Data;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HMI_LUTGearboxDialogController : ToolDialogController
{
	public Sprite GearboxUnset;
	public Sprite GearboxSet;

	private GameObject GearboxImage;
	private GameObject SetGearboxMode1Button;
	private GameObject SetGearboxMode2Button;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();

		SetGearboxMode1Button = contentArea.Find("SetGearboxMode1Button").gameObject;
		SetGearboxMode2Button = contentArea.Find("SetGearboxMode2Button").gameObject;
		GearboxImage = contentArea.Find("GearboxImage").gameObject;

		var hmiPageEnumerator = ComponentManager.Instance.GetStoredComponent(ComponentName);
		if(hmiPageEnumerator != null)
		{
			UpdateDialog(((SimulatorInterface.Enumerator)hmiPageEnumerator).index);
		}
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var enumerator = (SimulatorInterface.Enumerator)par;

			if (enumerator.name.ToLower() == ComponentName.ToLower())
			{
				UpdateDialog(enumerator.index);
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

	protected void UpdateDialog(int index)
	{
		if(index == 0)
		{
			GearboxImage.GetComponent<Image>().overrideSprite = GearboxUnset;
			SetGearboxMode1Button.SetActive(true);
			SetGearboxMode2Button.SetActive(false);
		}
		else if(index == 1)
		{
			GearboxImage.GetComponent<Image>().overrideSprite = GearboxSet;
			SetGearboxMode1Button.SetActive(false);
			SetGearboxMode2Button.SetActive(true);
		}
	}

	public void OnSetMode1()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 1 } });
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = "pump", data = new { enabled = true } });

	}

	public void OnSetMode2()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 0 } });
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = "pump", data = new { enabled = false } });
	}

}
