using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class EnumeratorTransitionBool
{
	public int FromIndex;
	public int ToIndex;
	public bool Value;
}


public class EnumeratorBoolListener : CPEventListener {

	public string ComponentName;
	public bool OnlyExactMatch = false;
	public string AnimVarName;
	public List<EnumeratorTransitionBool> Transitions;
	private Animator anim;
	private int LastIndex = 0;

	public void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var enumerator = (SimulatorInterface.Enumerator)par;

			if (enumerator.name.ToLower() == ComponentName.ToLower())
			{
				// Find right transition
				var transition = Transitions.FirstOrDefault(e => e.FromIndex == LastIndex && e.ToIndex == enumerator.index);
				if (transition == null && OnlyExactMatch == false)
				{
					// If going up in index (loading)
					var uptransition = Transitions.FirstOrDefault(e => e.FromIndex < e.ToIndex);
					if (uptransition.ToIndex <= enumerator.index && LastIndex == 0)
					{
						transition = uptransition;
					}
					else
					{
						// transition not found, just get first transition with right toIndex
						//transition = Transitions.FirstOrDefault(e => e.ToIndex == enumerator.index);
					}

					// Todo: check if this causes error in hpu
					//if (transition == null)
					//{
					//	transition = Transitions.FirstOrDefault(e => e.FromIndex == LastIndex);
					//}
				}

				//Play anim
				if (transition != null)
				{
					Debug.Log(string.Format("Setting Animation Variable Name [{0}] to {1}", AnimVarName,transition.Value));
					anim.SetBool(AnimVarName, transition.Value);
				}

				LastIndex = enumerator.index;
			}
		}
	}
}
