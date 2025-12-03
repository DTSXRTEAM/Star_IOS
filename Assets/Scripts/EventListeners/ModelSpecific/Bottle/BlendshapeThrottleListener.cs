using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendshapeThrottleListener : CPEventListener {

	[SerializeField] private string ComponentName;
	[SerializeField] private bool HideOnZero = true;
	[SerializeField] private float Scale = 1.0f;
	private SkinnedMeshRenderer skinnedRenderer;

	public void Awake()
	{
		skinnedRenderer = GetComponent<SkinnedMeshRenderer>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Throttle))
		{
			var throttle = (SimulatorInterface.Throttle)par;

			if (throttle.name.ToLower() == ComponentName.ToLower())
			{
				if(throttle.value > 0)
				{
					skinnedRenderer.enabled = true;
					skinnedRenderer.SetBlendShapeWeight(0, throttle.value*Scale);
				}
				else
				{
					if (HideOnZero)
					{
						skinnedRenderer.enabled = false;
					}
					skinnedRenderer.SetBlendShapeWeight(0, 0.0f);
				}
			}
		}
	}
}
