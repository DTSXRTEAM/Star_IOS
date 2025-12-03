using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HMI_PressureDialogController :ToolDialogController
{
	public Sprite pressureUnset;
	public Sprite pressureSet;
	public Sprite pumpUnset;
	public Sprite pumpSet;

	private GameObject PressureImage;
	private GameObject PumpText;
	private GameObject PumpImage;
	private GameObject PressureButton;
	private GameObject PumpOnButton;
	private GameObject PumpOffButton;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();
	
		PumpOnButton = contentArea.Find("PumpOnButton").gameObject;
		PumpOffButton = contentArea.Find("PumpOffButton").gameObject;
		PressureButton = contentArea.Find("PressureSetButton").gameObject;
		PumpImage = contentArea.Find("PumpImage").gameObject;
		PressureImage = contentArea.Find("PressureImage").gameObject;
		PumpText = contentArea.Find("PumpText").gameObject;

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
			PressureButton.SetActive(true);
			PumpText.SetActive(false);
			PumpImage.SetActive(false);
			PumpOnButton.SetActive(false);
			PumpOffButton.SetActive(false);
			contentAreaRect.sizeDelta = new Vector2(contentAreaRect.sizeDelta.x, 370);
		}
		else if(index == 1)
		{
			// change pressure image
			PressureImage.GetComponent<Image>().overrideSprite = pressureSet;
			PumpImage.GetComponent<Image>().overrideSprite = pumpUnset;

			PressureButton.SetActive(false);
			PumpText.SetActive(true);
			PumpImage.SetActive(true);
			PumpOnButton.SetActive(true);
			PumpOffButton.SetActive(false);
			contentAreaRect.sizeDelta = new Vector2(contentAreaRect.sizeDelta.x, 690);
		}
		else if (index == 2)
		{
			// change pressure image
			PumpImage.GetComponent<Image>().overrideSprite = pumpSet;

			PressureButton.SetActive(false);
			PumpText.SetActive(true);
			PumpImage.SetActive(true);
			PumpOnButton.SetActive(false);
			PumpOffButton.SetActive(true);
			contentAreaRect.sizeDelta = new Vector2(contentAreaRect.sizeDelta.x, 690);
		}
	}

	public void OnPressureSet()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 1 } });
	}

	public void OnPumpOn()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 2 } });
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = "pump", data = new { enabled = true } });

	}

	public void OnPumpOff()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 1 } });
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = "pump", data = new { enabled = false } });
	}

}
