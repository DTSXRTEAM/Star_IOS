using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LinearProgressBarModifierListener : CPEventListener
{
	[SerializeField] private string ComponentName;
	[SerializeField] private Image progressBarImage;
	[SerializeField] private GameObject progressBarText;
	[SerializeField] private string extension;

	private TextMeshProUGUI statusText;

	private void Awake()
	{
		if (progressBarText != null)
			statusText = progressBarText.GetComponent<TextMeshProUGUI>();
		else
			statusText = null;

		// Set initial progress
		var comp = ComponentManager.Instance.GetStoredComponent(ComponentName);
		if (comp != null)
		{
			UpdateDialog(((SimulatorInterface.Modifier)comp).value);
		}


	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Modifier))
		{
			var mod = (SimulatorInterface.Modifier)par;

			if (mod.name.ToLower() == ComponentName.ToLower())
			{
				UpdateDialog(mod.value);
			}
		}
	}

	private void UpdateDialog(float progress)
	{
		progressBarImage.fillAmount = (progress / 10.0f);

		if (statusText != null)
		{
			statusText.text = string.Format("Status: {0:0.0} {1}", progress, extension);
		}
	}
}
