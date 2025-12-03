using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingFloorListener : CPEventListener {

	public string ComponentName;
	public float ScaleOneValue;

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Floor))
		{
			var floor = (SimulatorInterface.Floor)par;

			if (floor.name.ToLower() == ComponentName.ToLower())
			{
				var scale = transform.localScale;
				scale.y = floor.value / ScaleOneValue;
				transform.localScale = scale;
			}
		}
	}
}
