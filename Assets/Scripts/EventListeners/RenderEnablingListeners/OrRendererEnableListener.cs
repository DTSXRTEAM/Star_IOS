using Cadpeople.Events;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrRendererEnableListener : CPEventListener
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
		if (par.GetType() == typeof(SimulatorInterface.Or))
		{
			var or = (SimulatorInterface.Or)par;

			if (or.name.ToLower() == ComponentName.ToLower())
			{
				if((or.value == 1 && Inverted == false) || (or.value == 0 && Inverted == true))
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
