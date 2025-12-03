using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SingleChoiceDialogController : ToolDialogController
{
//	public string stateComponentName;
	ToggleGroup ToggleGroupScript = null;

	protected override void Awake()
	{
		base.Awake();

		// Get current state
		ToggleGroupScript = GetComponent<ToggleGroup>();
		var breakerEnumerator = SimulatorInterface.ComponentManager.Instance.GetStoredComponent(ComponentName);
		if (breakerEnumerator != null)
		{
			var index = ((SimulatorInterface.Enumerator)breakerEnumerator).index;
			UpdateDialog(index);
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
		ToggleGroupScript.SetAllTogglesOff();

		// Get current state
		if (index > 0)
		{
			var list = transform.GetComponentsInChildren<Toggle>();
			list[index - 1].isOn = true;
		}
		else
		{
			ToggleGroupScript.SetAllTogglesOff();
		}
	}


	public void OnClose()
	{
		Toggle theActiveToggle = ToggleGroupScript.ActiveToggles().FirstOrDefault();
		int toggleIndex = 0;
		if (theActiveToggle != null)
		{
			toggleIndex = theActiveToggle.GetComponent<ToggleData>().index;
		}

		// Send result
		var evt = new SendObjectdata
		{
			name = ComponentName,
			data = new { index = toggleIndex }

		};
		sendOjectToServerEvent?.Raise(evt);
		GetComponent<BaseDialogTool>().OnClose();
	}
}
