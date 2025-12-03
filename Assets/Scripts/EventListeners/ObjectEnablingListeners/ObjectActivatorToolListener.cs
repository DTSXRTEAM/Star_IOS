using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Network;

public class ObjectActivatorToolListener : CPEventListener
{
	[SerializeField] private string ToolNameToActivate;
	[SerializeField] private GameObject ObjectToActivate;

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Toolbox))
		{
			var toolbox = (SimulatorInterface.Toolbox)par;
			if(toolbox.tool.ToLower() == ToolNameToActivate.ToLower() && ObjectToActivate != null)
			{
				ObjectToActivate.SetActive(true);
			}
			else
			{
				ObjectToActivate.SetActive(false);
			}
		}
	}
}
 
