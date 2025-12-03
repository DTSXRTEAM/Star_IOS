using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoseOilThrottleRenderEnableListener : CPEventListener
{
	[SerializeField] private string ComponentName;
	private new Renderer renderer = null;

	public void Awake()
	{
		renderer = GetComponent<Renderer>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Throttle))
		{
			var throttle = (SimulatorInterface.Throttle)par;

			if (throttle.name.ToLower() == ComponentName.ToLower())
			{
				if(renderer.enabled == false && throttle.value > 0)
				{
					renderer.enabled = true;
				}
			}
		}
	}
}
