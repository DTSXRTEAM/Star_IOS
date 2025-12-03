using Cadpeople.Events;
using Data;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManometerController : ToolDialogController
{
	[SerializeField] private GameObject ValueObject;

	private Text ValueText;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();
		ValueText = ValueObject.GetComponent<Text>();
	}

	public override void OnEventRaised(object par)
	{
		// Update display
		if (par.GetType() == typeof(SimulatorInterface.Manometer))
		{
			var manometer = (SimulatorInterface.Manometer)par;
			ValueText.text = manometer.display;
		}

		// Attach/detach manometer
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var enumerator = (SimulatorInterface.Enumerator)par;
			if(enumerator.name.ToLower() == "manometer-probe")
			{
				if (enumerator.index > 0)
					AttachManometer();
				else
					DetachManometer();
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

	public void AttachManometer()
	{
		var probe = new { probe = new ProbeData { component = "oil-pressure", output = "output0", name = "probe1" } };
		var sod = new SendObjectdata
		{
			name = "manometer",
			data = probe
		};
		sendOjectToServerEvent?.Raise(sod);
	}

	public void DetachManometer()
	{
		var probe = new { probe = new ProbeData { component = "0bar", output = "output0", name = "probe1" } };
		var sod = new SendObjectdata
		{
			name = "manometer",
			data = probe
		};
		sendOjectToServerEvent?.Raise(sod);
	}

}
