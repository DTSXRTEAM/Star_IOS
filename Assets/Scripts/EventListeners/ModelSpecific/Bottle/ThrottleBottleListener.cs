using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrottleBottleListener : CPEventListener {

	public string ComponentName;
	public GameObject ObjectToScale;

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Throttle))
		{
			var throttle = (SimulatorInterface.Throttle)par;

			if (throttle.name.ToLower() == ComponentName.ToLower())
			{
				Vector3 lTemp = ObjectToScale.transform.localScale;

				if (throttle.value > 0)
				{
					lTemp.y = throttle.value / 100.0f;
				}
				else
				{
					lTemp.y = 0.0f;
				}
				ObjectToScale.transform.localScale = lTemp;
			}
		}
	}
}
