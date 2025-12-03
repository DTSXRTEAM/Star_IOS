using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleListener : CPEventListener {

	public string ComponentName; 
	public string EnableAnim;
	public string DisableAnim;
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

				if (!string.IsNullOrWhiteSpace(EnableAnim) && toggle.enabled && anim != null)
				{
					anim.Play(EnableAnim);
				}
				else if (!string.IsNullOrWhiteSpace(DisableAnim) && !toggle.enabled && anim != null)
				{
					anim.Play(DisableAnim);
				}
			}
		}
	}
}
