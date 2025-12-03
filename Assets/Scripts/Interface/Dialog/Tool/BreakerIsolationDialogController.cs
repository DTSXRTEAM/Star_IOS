using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakerIsolationDialogController : ToolDialogController
{
	public Sprite breakerIsolated;
	public Sprite breakerNotIsolated;

	private GameObject breakerImage;
	private GameObject breakerAddIsolationButton;
	private GameObject breakerRemoveIsolationButton;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();

		breakerAddIsolationButton = contentArea.Find("BreakerAddIsolationButton").gameObject;
		breakerRemoveIsolationButton = contentArea.Find("BreakerRemoveIsolationButton").gameObject;
		breakerImage = contentArea.Find("BreakerImage").gameObject;

		var breakerToogle = ComponentManager.Instance.GetStoredComponent(ComponentName);
		if(breakerToogle != null)
		{
			UpdateDialog(((SimulatorInterface.Toggle)breakerToogle).enabled);
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

	private void UpdateDialog(bool breakerNotApplied)
	{
		if(breakerNotApplied)
		{
			breakerImage.GetComponent<Image>().overrideSprite = breakerNotIsolated;
			breakerAddIsolationButton.SetActive(true);
			breakerRemoveIsolationButton.SetActive(false);
		}
		else if (breakerNotApplied == false)
		{
			breakerImage.GetComponent<Image>().overrideSprite = breakerNotIsolated;
			breakerAddIsolationButton.SetActive(false);
			breakerRemoveIsolationButton.SetActive(true);
		}
	}

	public void OnAddBreakerIsolation()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { enabled = false } });
	}

	public void OnRemoveBreakerIsolation()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { enabled = true } });
	}
}
