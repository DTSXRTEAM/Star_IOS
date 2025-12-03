using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

[Serializable]
public class DropdownMapper
{
	public TMP_Dropdown DropdownMenu;
	public string ComponentName; 
}


public class MultipleDropdownDialogController : ToolDialogController
{
	[SerializeField] private List<DropdownMapper> Dropdowns;

	protected override void Awake()
	{
		base.Awake();

		// Get current state
		Dropdowns.ForEach(dd =>
		{
			var enumerator = SimulatorInterface.ComponentManager.Instance.GetStoredComponent(dd.ComponentName);
			if(enumerator != null)
				dd.DropdownMenu.value = ((SimulatorInterface.Enumerator)enumerator).index;
		});
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var enumerator = (SimulatorInterface.Enumerator)par;
			var match = Dropdowns.FirstOrDefault(e => e.ComponentName.ToLower() == enumerator.name.ToLower());

			if (match != null)
			{
				match.DropdownMenu.value = enumerator.index;
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

	public void OnClose()
	{
		Dropdowns.ForEach(dd =>
		{
			// Send result
			var evt = new SendObjectdata
			{
				name = dd.ComponentName,
				data = new { index = dd.DropdownMenu.value }
			};
			sendOjectToServerEvent?.Raise(evt);
		});

		GetComponent<BaseDialogTool>().OnClose();
	}
}
