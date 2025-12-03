using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotorLockPinBoundsListener : CPEventListener
{

	[SerializeField] private string ComponentName;

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Bounds))
		{
			var bounds = (SimulatorInterface.Bounds)par;

			if (bounds.name.ToLower() == ComponentName.ToLower())
			{
				Vector3 position = transform.localPosition;

				if (bounds.value > 0)
				{
					position.y =  -(bounds.value * 0.005f);
				}
				else
				{
					position.y = 0.0f;
				}
				transform.localPosition = position;
			}
		}
	}
}
