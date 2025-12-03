using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToogleSimpleAudioListener : CPEventListener
{
	[SerializeField] private string ComponentName;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private bool inverted;

	public override void OnEventRaised(object par)
	{
		// Is it toggle component
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var toggle = (SimulatorInterface.Toggle)par;

			// Is it the right toggle name id
			if (toggle.name.ToLower() == ComponentName.ToLower())
			{
				if (toggle.name.ToLower() == ComponentName.ToLower() && audioSource != null && toggle.enabled)
				{
					if(inverted)
						audioSource.Stop();
					else
						audioSource.Play();
				}
				else if (toggle.name.ToLower() == ComponentName.ToLower() && audioSource != null && !toggle.enabled)
				{
					if (inverted)
						audioSource.Play();
					else
						audioSource.Stop();
				}
			}
		}
	}

	public void TurnOff()
	{
		audioSource.Stop();
	}

	//public IEnumerator PitchUp()
	//{
	//	audioSource.Play();
	//	var totaltime = PitchUpCurve.keys[PitchUpCurve.length - 1].time;
	//	float timer = 0.0f;
	//	while (timer <= totaltime)
	//	{
	//		audioSource.pitch = PitchUpCurve.Evaluate(timer);
	//		timer += Time.deltaTime;
	//		yield return null;
	//	}
	//}

	//public IEnumerator PitchDown()
	//{
	//	var totaltime = PitchDownCurve.keys[PitchDownCurve.length - 1].time;
	//	float timer = 0.0f;
	//	while (timer <= totaltime)
	//	{
	//		audioSource.pitch = PitchDownCurve.Evaluate(timer);
	//		timer += Time.deltaTime;
	//		yield return null;
	//	}

	//	audioSource.Stop();
	//}
}
