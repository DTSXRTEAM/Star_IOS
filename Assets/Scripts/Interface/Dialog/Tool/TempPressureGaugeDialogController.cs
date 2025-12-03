using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempPressureGaugeDialogController :ToolDialogController
{
	[SerializeField]private Transform PressureArrowImage;
	[SerializeField] private Transform TemperatureArrowImage;
	private GameObject CloseButton;
	private GameObject WarmUpButton;
	private GameObject WarmUpButtonGrayed;
	private GameObject WarmUpText;

	private float targetPresAngle;
	private float currentPresAngle;
	private float currentTempAngle;
	private float targetTempAngle;
	private bool rotatingPres;
	private bool rotatingTemp;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();
		CloseButton = contentArea.Find("CloseButton").gameObject;
		WarmUpButton = contentArea.Find("WarmUpButton").gameObject;
		WarmUpButtonGrayed = contentArea.Find("WarmUpButtonGrayed").gameObject;
		WarmUpText = contentArea.Find("WarmUpText").gameObject;

		var tg = ComponentManager.Instance.GetStoredComponent(ComponentName);
		if(tg != null)
		{
			UpdateDialog(((SimulatorInterface.Toggle)tg).enabled);
		}

		currentTempAngle = 190;
		currentPresAngle = 0;
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var tg = (SimulatorInterface.Toggle)par;

			if (tg.name.ToLower() == ComponentName.ToLower())
			{
				UpdateDialog(tg.enabled);
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

	protected void UpdateDialog(bool warmedUp)
	{
		if (warmedUp == false)
		{
			targetPresAngle = 10;
			targetTempAngle = 158f;

			WarmUpButton.SetActive(true);
			WarmUpButtonGrayed.SetActive(false);
			WarmUpText.SetActive(false);
		}
		else
		{
			targetPresAngle = -18;
			targetTempAngle = 136.5f;

			WarmUpButton.SetActive(false);
			WarmUpButtonGrayed.SetActive(true);
			WarmUpText.SetActive(true);
		}
		rotatingPres = true;
		rotatingTemp = true;
	}

	public void OnWarmUp()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { enabled = true } });
	}

	private void Update()
	{
		if (rotatingPres)
		{
			if (Math.Abs(currentPresAngle - targetPresAngle) > 0.05f)
			{
				currentPresAngle = Mathf.Lerp (currentPresAngle, targetPresAngle, Time.deltaTime);
				PressureArrowImage.eulerAngles = new Vector3(0, 0, currentPresAngle);
			}
			else
			{
				PressureArrowImage.eulerAngles = new Vector3(0, 0, targetPresAngle);
				rotatingPres = false;
			}
		}

		if (rotatingTemp)
		{
			if (Math.Abs(currentTempAngle - targetTempAngle) > 0.05f)
			{
				currentTempAngle = Mathf.Lerp(currentTempAngle, targetTempAngle, Time.deltaTime);
				TemperatureArrowImage.eulerAngles = new Vector3(0, 0, currentTempAngle);
			}
			else
			{
				TemperatureArrowImage.eulerAngles = new Vector3(0, 0, targetTempAngle);
				rotatingTemp = false;
			}
		}
	}
}
