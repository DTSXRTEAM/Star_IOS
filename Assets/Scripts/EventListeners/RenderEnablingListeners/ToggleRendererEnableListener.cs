using Cadpeople.Events;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRendererEnableListener : CPEventListener
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
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var tg = (SimulatorInterface.Toggle)par;

			if (tg.name.ToLower() == ComponentName.ToLower())
			{
				if((tg.enabled == true && Inverted == false) || (tg.enabled == false && Inverted == true))
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
