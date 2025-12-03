using Cadpeople.Events;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class EnumeratorAcceptCriteria
{
	public EnumeratorAcceptCriteria()
	{
		Index = 0;
	}

	public string EnumeratorComponentName;
	public int Index;

	[HideInInspector]
	public bool Accepted;
}

public class EnumeratorExRendererEnableListener : CPEventListener
{
	public List<EnumeratorAcceptCriteria> EnableCriterias;

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

			// Set cached accept
			var ec = EnableCriterias.FirstOrDefault(e => e.EnumeratorComponentName.ToLower() == en.name.ToLower());
			if(ec != null)
			{
				ec.Accepted = en.index == ec.Index;
			}

			bool enable = true;					   
			EnableCriterias.ForEach(x =>
			{
				if (!x.Accepted)
					enable = false;

			});

			if(enable)
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
