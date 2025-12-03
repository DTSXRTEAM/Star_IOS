using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelFader : CPEventListener
{
	private Coroutine runningCoroutine;

	public AnimationCurve FadeInCurve;
	public AnimationCurve FadeOutCurve;


	public override void OnEventRaised(object parameter)
	{
		bool fadeout = false;
		if (parameter != null)
			fadeout = (bool)parameter;

		if(fadeout)
		{
			if (runningCoroutine != null)
				StopCoroutine(runningCoroutine);

			runningCoroutine = StartCoroutine(FadeIn());			
		}
		else
		{
			if (runningCoroutine != null)
				StopCoroutine(runningCoroutine);

			runningCoroutine = StartCoroutine(FadeOut());
		}
	}

	IEnumerator FadeIn()
	{
		var canvasgroup = GetComponent<CanvasGroup>();
		var totaltime = FadeInCurve.keys[FadeInCurve.length - 1].time;
		float timer = 3.0f;
		while (timer <= totaltime)
		{
			canvasgroup.alpha = FadeInCurve.Evaluate(timer);
			timer += Time.deltaTime;
			yield return null;
		}
		canvasgroup.interactable = false;
		canvasgroup.alpha = 1;
	}

	IEnumerator FadeOut()
	{
		var canvasgroup = GetComponent<CanvasGroup>();
		var totaltime = FadeOutCurve.keys[FadeOutCurve.length-1].time;
		float timer = 0.0f;
		while (timer <= totaltime)
		{
			canvasgroup.alpha = FadeOutCurve.Evaluate(timer);
			timer += Time.deltaTime;
			yield return null;
		}
		canvasgroup.interactable = false;
		canvasgroup.alpha = 0;
	}
}
