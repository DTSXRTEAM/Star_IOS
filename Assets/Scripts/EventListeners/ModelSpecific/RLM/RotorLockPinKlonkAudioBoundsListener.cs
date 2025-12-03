using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotorLockPinKlonkAudioBoundsListener : CPEventListener
{
	[SerializeField] private string ComponentName;
	[SerializeField] private AudioSource audioSource;

	private float lastvalue = 0;
	private float tagetVal = 20;


	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Bounds))
		{
			var bounds = (SimulatorInterface.Bounds)par;

			if (bounds.name.ToLower() == ComponentName.ToLower())
			{
				if((bounds.value-tagetVal) < 0.1f && (lastvalue - tagetVal) > 0.1f)
				{
					audioSource.Play();
				}
				lastvalue = bounds.value;
			}
		}
	}
}
