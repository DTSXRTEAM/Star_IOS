using Cadpeople.Events;
using SimulatorInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyringeOilThrottleListener : CPEventListener
{
	[SerializeField] private string ComponentName;
	[SerializeField] private Image OilImage;

	private void Awake()
	{
		var comp = ComponentManager.Instance.GetStoredComponent(ComponentName);
		if (comp != null)
		{
			OilImage.fillAmount = ((SimulatorInterface.Throttle)comp).value / 100.0f; ;
		}
	}


	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Throttle))
		{
			var throttle = (SimulatorInterface.Throttle)par;

			if (throttle.name.ToLower() == ComponentName.ToLower())
			{
				OilImage.fillAmount = throttle.value/100.0f;
			}
		}
	}
}
