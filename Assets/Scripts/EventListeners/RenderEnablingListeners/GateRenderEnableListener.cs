using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateRenderEnableListener : CPEventListener
{
	[SerializeField] private string ComponentName;
	[SerializeField] private bool Inverted = false;

	private new Renderer renderer = null;
	private new Collider collider = null;

	public void Awake()
	{
		renderer = GetComponent<Renderer>();
		collider = GetComponent<Collider>();
	}

	public override void OnEventRaised(object par)
	{
		// Is it toggle component
		if (par.GetType() == typeof(SimulatorInterface.Gate))
		{
			var gate = (SimulatorInterface.Gate)par;

			// Is it the right toggle name id
			if (gate.name.ToLower() == ComponentName.ToLower())
			{
				if ((gate.enabled == true && Inverted == false) || (gate.enabled == false && Inverted == true))
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
