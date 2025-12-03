using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolumeController : CPEventListener
{
	[SerializeField] private AnimationCurve FadeInCurve;
	[SerializeField] private AnimationCurve FadeOutCurve;
	[SerializeField] private AudioMixer masterMixer;

	private Coroutine runningCoroutine;

	public override void OnEventRaised(object parameter)
	{
		bool fadeout = false;
		if (parameter != null)
			fadeout = (bool)parameter;

		if(fadeout)
		{
			if (runningCoroutine != null)
				StopCoroutine(runningCoroutine);

			runningCoroutine = StartCoroutine(FadeOut());			
		}
		else
		{
			if (runningCoroutine != null)
				StopCoroutine(runningCoroutine);

			runningCoroutine = StartCoroutine(FadeIn());
		}
	}

	IEnumerator FadeIn()
	{
		var totaltime = FadeInCurve.keys[FadeInCurve.length - 1].time;
		float timer = 0.0f;
		while (timer <= totaltime)
		{
			masterMixer.SetFloat("MasterVolume", FadeInCurve.Evaluate(timer));
			timer += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator FadeOut()
	{
		var totaltime = FadeOutCurve.keys[FadeOutCurve.length - 1].time;
		float timer = 0.0f;
		while (timer <= totaltime)
		{
			masterMixer.SetFloat("MasterVolume", FadeInCurve.Evaluate(timer));
			timer += Time.deltaTime;
			yield return null;
		}
	}
}
