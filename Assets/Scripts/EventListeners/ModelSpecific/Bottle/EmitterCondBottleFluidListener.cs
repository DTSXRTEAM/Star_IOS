using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterCondBottleFluidListener : CPEventListener {

	public string ComponentName;

	private MeshRenderer oilRenderer = null;
	public void Awake()
	{
		var oil = transform.GetChild(0);
		if (oil != null)
		{
			oilRenderer = oil.GetComponent<MeshRenderer>();
		}
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.EmitterCond))
		{
			var emitterCond = (SimulatorInterface.EmitterCond)par;

			if (emitterCond.name.ToLower() == ComponentName.ToLower())
			{
				Vector3 lTemp = transform.localScale;


				if (emitterCond.data.fluid > 0)
				{
					if(oilRenderer != null)
					{
						oilRenderer.enabled = true;
					}
					lTemp.y = 1.0f;
				}
				else
				{
					lTemp.y = 0.0f;
					if (oilRenderer != null)
					{
						oilRenderer.enabled = false;
					}
				}
				transform.localScale = lTemp;
			}
		}
	}
}
