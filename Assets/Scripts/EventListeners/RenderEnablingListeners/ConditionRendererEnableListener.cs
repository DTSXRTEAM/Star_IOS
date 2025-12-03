using Cadpeople.Events;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionRendererEnableListener : CPEventListener
{
	public string ComponentName;

	private new Renderer renderer = null;
	private new Collider collider = null;
	public void Awake()
	{
		renderer = GetComponent<Renderer>();
		collider = GetComponent<Collider>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Condition))
		{
			var cond = (SimulatorInterface.Condition)par;

			if (cond.name.ToLower() == ComponentName.ToLower())
			{
				if(cond.value == "1")
				{
					renderer.enabled = true;
					if (collider != null)
						collider.enabled = true;
				}
				else
				{
					renderer.enabled = false;
					if (collider != null)
						collider.enabled = false;
				}
			}
		}
	}
}
