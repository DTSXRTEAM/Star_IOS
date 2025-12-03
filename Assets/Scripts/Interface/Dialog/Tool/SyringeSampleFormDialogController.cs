using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SyringeSampleFormDialogController : ToolDialogController
{
	[SerializeField] private TMP_InputField siteName;
	[SerializeField] private TMP_InputField wtgID;
	[SerializeField] private TMP_InputField transformerType;
	[SerializeField] private TMP_InputField transformerSerialNumber;
	[SerializeField] private TMP_InputField date;
	[SerializeField] private TMP_InputField oilTemperature;
	[SerializeField] private TMP_InputField technicalName;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();

		var formData = ComponentManager.Instance.GetStoredComponent(ComponentName);
		if(formData != null)
		{
			UpdateDialog(((SimulatorInterface.State)formData).value);
		}
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.State))
		{
			var formData = (SimulatorInterface.State)par;

			if (formData.name.ToLower() == ComponentName.ToLower())
			{
				UpdateDialog(formData.value);
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

	protected void UpdateDialog(ValueData data)
	{
		siteName.text = data.siteName;
		wtgID.text = data.wtgId;
		transformerType.text = data.transformerType;
		transformerSerialNumber.text = data.transformerSerial;
		date.text = data.date;
		oilTemperature.text = data.oilTemp;
		technicalName.text = data.technicalName;
	}

	public void OnOk()
	{
		var data = new ValueData
		{
			siteName = siteName.text,
			wtgId = wtgID.text,
			transformerType = transformerType.text,
			transformerSerial = transformerSerialNumber.text,
			date = date.text,
			oilTemp = oilTemperature.text,
			technicalName = technicalName.text
		};

		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { value = data } });

		GetComponent<BaseDialogTool>().OnClose();
	}
}
