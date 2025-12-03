using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorExListener : CPEventListener {

	[SerializeField] private string ComponentName;
	[SerializeField] private string AnimBoolName;

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

			if (indicator.name.ToLower() == ComponentName.ToLower() && indicator.active && anim != null)
			{
				anim.SetBool(AnimBoolName, true);
			}
			else if(indicator.name.ToLower() == ComponentName.ToLower() && !indicator.active && anim != null)
			{
				anim.SetBool(AnimBoolName, false);
			}
		}
	}
}
