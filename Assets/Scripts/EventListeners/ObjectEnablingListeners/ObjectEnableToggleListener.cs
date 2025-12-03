using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Network;

public class ObjectEnableToggleListener : CPEventListener
{
	[SerializeField] private string ToggleNameToActivate;
	[SerializeField] private GameObject ObjectToActivate;

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var tg = (SimulatorInterface.Toggle)par;
			if(tg.name.ToLower() == ToggleNameToActivate.ToLower())
			{
				ObjectToActivate.SetActive(tg.enabled);
			}
		}
	}
}
 
