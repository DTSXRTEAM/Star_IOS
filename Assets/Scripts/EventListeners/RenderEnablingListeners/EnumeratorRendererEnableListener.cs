using Cadpeople.Events;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnumeratorRendererEnableListener : CPEventListener
{
	public string ComponentName;
	public List<string> EnabledIndexes;

	private new Renderer renderer = null;
	private new Collider collider = null;
	public void Awake()
	{
		renderer = GetComponent<Renderer>();
		collider = GetComponent<Collider>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var en = (SimulatorInterface.Enumerator)par;

			if (en.name.ToLower() == ComponentName.ToLower())
			{
				if(EnabledIndexes.FirstOrDefault(e => e == en.index.ToString()) != null)
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
