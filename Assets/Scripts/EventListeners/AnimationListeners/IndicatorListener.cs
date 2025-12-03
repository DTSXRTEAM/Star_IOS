using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorListener : CPEventListener {

	public string ComponentName; 
	public string ActiveAnim;
	public string DeactiveAnim;

	private Animator anim;

	public void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Indicator))
		{
			var indicator = (SimulatorInterface.Indicator)par;

			if (indicator.name.ToLower() == ComponentName.ToLower() && !string.IsNullOrWhiteSpace(ActiveAnim) && indicator.active && anim != null)
			{
				anim.Play(ActiveAnim);
				Debug.Log("Running animation: " + ActiveAnim);
			}
			else if(indicator.name.ToLower() == ComponentName.ToLower() && !string.IsNullOrWhiteSpace(DeactiveAnim) && !indicator.active && anim != null)
			{
				anim.Play(DeactiveAnim);
				Debug.Log("Running animation: " + DeactiveAnim);
			}
		}
	}
}
