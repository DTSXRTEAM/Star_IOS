using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilLevelThrottleListener : CPEventListener {

	public string ComponentName;
	private Renderer rend;

	private void Awake()
	{
		rend = GetComponent<Renderer>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Throttle))
		{
			var throttle = (SimulatorInterface.Throttle)par;

			if (throttle.name.ToLower() == ComponentName.ToLower())
			{
				float cutOffValue = 0;
				if (throttle.value > 69)
				{
					cutOffValue = (1.0f - (0.178f + ((throttle.value - 70) * 0.03625f)));
					rend.material.SetFloat("_Cutoff", cutOffValue);
				}
				else
				{
					rend.material.SetFloat("_Cutoff", 1f);
				}
			}
		}
	}
}
