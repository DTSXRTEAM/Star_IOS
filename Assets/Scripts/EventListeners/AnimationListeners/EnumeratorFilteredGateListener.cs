using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumeratorFilteredGateListener : CPEventListener {

	public string ComponentName;
	public string FilterEnumeratorName;
	public int EnumeratorIndexGreaterThan;
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
		if (par.GetType() == typeof(SimulatorInterface.Gate))
		{
			var gate = (SimulatorInterface.Gate)par;

			// Is it the right toggle name id
			if (gate.name.ToLower() == ComponentName.ToLower())
			{
				Debug.Log("Toggle : " + gate.name + ", Value : " + gate.enabled);

				var en = SimulatorInterface.ComponentManager.Instance.GetStoredComponent(FilterEnumeratorName) as SimulatorInterface.Enumerator;
				if (en.index > EnumeratorIndexGreaterThan)
				{
					if (!string.IsNullOrWhiteSpace(EnableAnim) && gate.enabled && anim != null)
					{
						anim.Play(EnableAnim);
					}
					else if (!string.IsNullOrWhiteSpace(DisableAnim) && !gate.enabled && anim != null)
					{
						anim.Play(DisableAnim);
					}
				}
			}
		}
	}
}
