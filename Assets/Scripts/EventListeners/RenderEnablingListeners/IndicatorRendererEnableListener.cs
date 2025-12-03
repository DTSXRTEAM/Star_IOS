using Cadpeople.Events;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorRendererEnableListener : CPEventListener
{
	public string ComponentName;
	public bool Inverted = false;
	private new Renderer renderer = null;
	private new Collider collider = null;
	public void Awake()
	{
		renderer = GetComponent<Renderer>();
		collider = GetComponent<Collider>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Indicator))
		{
			var indicator = (SimulatorInterface.Indicator)par;

			if (indicator.name.ToLower() == ComponentName.ToLower())
			{
				if((indicator.active == true && Inverted == false) || (indicator.active == false && Inverted == true))
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
