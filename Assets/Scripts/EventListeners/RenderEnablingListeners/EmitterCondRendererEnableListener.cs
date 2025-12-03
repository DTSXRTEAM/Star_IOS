using Cadpeople.Events;
using Data;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterCondRendererEnableListener : CPEventListener
{
	public string ComponentName;
	public AcceptCriteria[] AcceptCriterias;

	private new Renderer renderer = null;
	private new Collider collider = null;
	public void Awake()
	{
		renderer = GetComponent<Renderer>();
		collider = GetComponent<Collider>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.EmitterCond))
		{
			var emitterCond = (SimulatorInterface.EmitterCond)par;

			if (emitterCond.name.ToLower() == ComponentName.ToLower())
			{
				if (CheckCriterias(emitterCond))
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

	private bool CheckCriterias(EmitterCond emitterCond)
	{
		bool ok = true;

		for (int i = 0; i < AcceptCriterias.Length; i++)
		{
			if (((int)emitterCond.data[AcceptCriterias[i].DataParameterKey]) != AcceptCriterias[i].DataParameterValue)
			{
				ok = false;
			}
		}
		return ok;
	}
}
