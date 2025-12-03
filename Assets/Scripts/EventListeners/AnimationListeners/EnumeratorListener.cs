using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumeratorListener : CPEventListener {

	public string ComponentName;
	public int RunAnimationIfValue;
	public string EnableAnim;
//	public string DisableAnim;

	private Animator anim;

	public void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var enumerator = (SimulatorInterface.Enumerator)par;

			if (enumerator.name.ToLower() == ComponentName.ToLower() && enumerator.index == RunAnimationIfValue)
			{
				anim.Play(EnableAnim);
			}
			//else if (enumerator.name.ToLower() == ComponentName.ToLower() && enumerator.index < RunAnimationIfValue)
			//{
			//	anim.Play(DisableAnim);
			//}
		}
	}
}
