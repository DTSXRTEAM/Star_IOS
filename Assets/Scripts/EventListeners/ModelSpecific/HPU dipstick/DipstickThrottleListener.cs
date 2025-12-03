using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DipstickThrottleListener : CPEventListener {

	public string ComponentName;

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Throttle))
		{
			var throttle = (SimulatorInterface.Throttle)par;

			if (throttle.name.ToLower() == ComponentName.ToLower())
			{
				Vector3 scale = transform.localScale;

				if (throttle.value > 0)
				{
					scale.y = 1.0f + (throttle.value * 0.0285f);
				}
				else
				{
					scale.y = 0.0f;
				}
				transform.localScale = scale;
			}
		}
	}
}
