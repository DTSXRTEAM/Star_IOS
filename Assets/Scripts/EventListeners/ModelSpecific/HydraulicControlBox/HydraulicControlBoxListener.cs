using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraulicControlBoxListener : CPEventListener
{
	[SerializeField] private string HydralicComponentName;
	[SerializeField] private string Pump1ComponentName;
	[SerializeField] private string Pump2ComponentName;
	[SerializeField] private string Pump3ComponentName;

	[SerializeField] private Transform HydraulicSwitch;
	[SerializeField] private Transform Pump1Switch;
	[SerializeField] private Transform Pump2Switch;
	[SerializeField] private Transform Pump3Switch;

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var enumerator = (SimulatorInterface.Enumerator)par;
			if (enumerator.name.ToLower() == HydralicComponentName.ToLower())
			{
				switch(enumerator.index)
				{
					case 0:
						HydraulicSwitch.localEulerAngles = new Vector3(0, 0, 0);
						break;
					case 1:
						HydraulicSwitch.localEulerAngles = new Vector3(0, 45, 0);
						break;
					case 2:
						HydraulicSwitch.localEulerAngles = new Vector3(0, -45, 0);
						break;
				}
			}
		}

		// Pumps
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var tg = (SimulatorInterface.Toggle)par;

			// Pump 1
			if (tg.name.ToLower() == Pump1ComponentName.ToLower())
			{
				if(tg.enabled)
				{
					Pump1Switch.localEulerAngles = new Vector3(0, 0, 0);
				}
				else
				{
					Pump1Switch.localEulerAngles = new Vector3(0, 45, 0);
				}
			}
			// Pump 2
			else if (tg.name.ToLower() == Pump2ComponentName.ToLower())
			{
				if (tg.enabled)
				{
					Pump2Switch.localEulerAngles = new Vector3(0, 0, 0);
				}
				else
				{
					Pump2Switch.localEulerAngles = new Vector3(0, 45, 0);
				}
			}
			// Pump 3
			else if (tg.name.ToLower() == Pump3ComponentName.ToLower())
			{
				if (tg.enabled)
				{
					Pump3Switch.localEulerAngles = new Vector3(0, 0, 0);
				}
				else
				{
					Pump3Switch.localEulerAngles = new Vector3(0, 45, 0);
				}
			}
		}
	}
}
