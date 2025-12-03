using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionAudioListener : CPEventListener
{
	public string ComponentName;
	public AudioSource audioSource;

	public override void OnEventRaised(object par)
	{
		// Is it toggle component
		if (par.GetType() == typeof(SimulatorInterface.Condition))
		{
			var cond = (SimulatorInterface.Condition)par;

			// Is it the right toggle name id
			if (cond.name.ToLower() == ComponentName.ToLower())
			{
				if (cond.value == "1")
				{
					audioSource.Play();
					
				}
				else
				{
					audioSource.Stop();
				}
			}
		}
	}

	public void TurnOff()
	{
		audioSource.Stop();
	}

}
