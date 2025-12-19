using Cadpeople.Events;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnumeratorObjectEnableListener : CPEventListener
{
	[SerializeField] private string ComponentName;
	[SerializeField] private List<string> EnabledIndexes;
	[SerializeField] private GameObject ObjectToEnable;

	[SerializeField] float delay = 0;


	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var en = (SimulatorInterface.Enumerator)par;

			if (en.name.ToLower() == ComponentName.ToLower())
			{
				if(EnabledIndexes.FirstOrDefault(e => e == en.index.ToString()) != null)
				{
					if (delay > 0.1)
					{
						StartCoroutine(ActivateObjectCoroutine(true));
					}
					else
					{
						ObjectToEnable?.SetActive(true);
					}
				}
				else
				{
					if (delay > 0.1)
					{
						StartCoroutine(ActivateObjectCoroutine(false));
					}
					else
					{
						ObjectToEnable?.SetActive(false);
					}
				}
			}
		}
	}

	IEnumerator ActivateObjectCoroutine(bool activate)
	{
		yield return new WaitForSeconds(delay);
		ObjectToEnable?.SetActive(activate);
	}
}
