using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrottleListener : CPEventListener {

	public string ComponentName;

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Throttle))
		{
			var throttle = (SimulatorInterface.Throttle)par;

			if (throttle.name.ToLower() == ComponentName.ToLower())
			{
				var scale = transform.localScale;
				scale.y = throttle.value / 100.0f;
				transform.localScale = scale;
			}
		}
	}
}
