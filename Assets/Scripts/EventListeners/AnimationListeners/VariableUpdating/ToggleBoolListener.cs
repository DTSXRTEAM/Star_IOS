using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBoolListener : CPEventListener {

	[SerializeField] private string ComponentName;
	[SerializeField] private string AnimVarName;
	[SerializeField] private bool Invert;
	private Animator anim;

	public void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public override void OnEventRaised(object par)
	{
		// Is it toggle component
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var toggle = (SimulatorInterface.Toggle)par;

			// Is it the right toggle name id
			if (toggle.name.ToLower() == ComponentName.ToLower())
			{
				Debug.Log("Toggle : " + toggle.name + ", Value : " + toggle.enabled);

				if (!string.IsNullOrWhiteSpace(AnimVarName) && toggle.enabled && anim != null)
				{
					anim.SetBool(AnimVarName, Invert ? false : true);
				}
				else if (!string.IsNullOrWhiteSpace(AnimVarName) && !toggle.enabled && anim != null)
				{
					anim.SetBool(AnimVarName, Invert ? true : false);
				}
			}
		}
	}
}
