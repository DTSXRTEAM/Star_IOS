using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterCondListener : CPEventListener {

	public string ComponentName;
	public AcceptCriteria[] AcceptCriterias;
	public string EnableAnim;
	public string DisableAnim;

	private Animator anim;

	public void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(EmitterCond))
		{
			var emitterCond = (EmitterCond)par;

			if(emitterCond.name.ToLower() == ComponentName.ToLower())
			{
				if (CheckCriterias(emitterCond))
				{
					anim.Play(EnableAnim);
				}
				else
				{
					anim.Play(DisableAnim);
				}
			}
		}
	}

	private bool CheckCriterias(EmitterCond emitterCond)
	{
		bool ok = true;

		for(int i = 0;i < AcceptCriterias.Length;i++)
		{
			if (((int)emitterCond.data[AcceptCriterias[i].DataParameterKey]) != AcceptCriterias[i].DataParameterValue)
			{
				ok = false;
			}
		}
		return ok;
	}
}
