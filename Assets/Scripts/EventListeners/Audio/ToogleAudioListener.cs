using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToogleAudioListener : CPEventListener
{
	public string ComponentName;
	public AudioSource audioSource;
	public AnimationCurve PitchUpCurve;
	public AnimationCurve PitchDownCurve;

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
					StartCoroutine(PitchUp());
					
				}
				else if (toggle.name.ToLower() == ComponentName.ToLower() && audioSource != null && !toggle.enabled)
				{
					StartCoroutine(PitchDown());
				}
			}
		}
	}

	public void TurnOff()
	{
		audioSource.Stop();
	}

	public IEnumerator PitchUp()
	{
		audioSource.Play();
		var totaltime = PitchUpCurve.keys[PitchUpCurve.length - 1].time;
		float timer = 0.0f;
		while (timer <= totaltime)
		{
			audioSource.pitch = PitchUpCurve.Evaluate(timer);
			timer += Time.deltaTime;
			yield return null;
		}
	}

	public IEnumerator PitchDown()
	{
		var totaltime = PitchDownCurve.keys[PitchDownCurve.length - 1].time;
		float timer = 0.0f;
		while (timer <= totaltime)
		{
			audioSource.pitch = PitchDownCurve.Evaluate(timer);
			timer += Time.deltaTime;
			yield return null;
		}

		audioSource.Stop();
	}
}
