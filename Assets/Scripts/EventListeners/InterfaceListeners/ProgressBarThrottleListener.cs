using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnumeratorCondition
{
	public string enumeratorComponentName;
	public int index;
}

public class ProgressBarThrottleListener : CPEventListener
{
	[SerializeField] private EnumeratorCondition[] EnumeratorConditions;
	[SerializeField] private string ComponentName;
	[SerializeField] private string ProgressText;
	[SerializeField] private GameEvent showProgressBarEvent;

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Throttle))
		{
			bool valid = true;
			if (EnumeratorConditions != null && EnumeratorConditions.Length > 0)
			{
				valid = false;
				foreach (var ec in EnumeratorConditions)
				{
					var en = SimulatorInterface.ComponentManager.Instance.GetStoredComponent(ec.enumeratorComponentName) as SimulatorInterface.Enumerator;
					if (en != null && en.index == ec.index)
					{
						valid = true;
					}
					else
					{
						valid = false;
						break;
					}
				}
			}

			if (valid)
			{
				var throttle = (SimulatorInterface.Throttle)par;

				if (throttle.name.ToLower() == ComponentName.ToLower())
				{
					showProgressBarEvent?.Raise(new ProgressData { progress = (int)throttle.value, progressText = ProgressText });
				}
			}
			else
			{
				var throttle = (SimulatorInterface.Throttle)par;
				if (throttle.name.ToLower() == ComponentName.ToLower())
				{
					showProgressBarEvent?.Raise(new ProgressData { progress = 0, progressText = "" });
				}

			}
		}
	}
}
