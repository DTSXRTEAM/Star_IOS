using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorSwapperGateListener : CPEventListener
{
	public string ComponentName;
	[SerializeField] private Color NewColor;
	[SerializeField] private int MaterialNumber = 0;

	private Color OrgColor;
	private Material mat = null;

	private void Awake()
	{
		if (GetComponent<Renderer>().materials.Length > MaterialNumber)
		{
			mat = GetComponent<Renderer>().materials[MaterialNumber];
			OrgColor = mat.color;
		}
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

				if (gate.enabled)
				{
					SetNewColor();
				}
				else
				{
					SetOrgColor();
				}

			}
		}
		else if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var tg = (SimulatorInterface.Toggle)par;

			// Is it the right toggle name id
			if (tg.name.ToLower() == ComponentName.ToLower())
			{
				Debug.Log("Toggle : " + tg.name + ", Value : " + tg.enabled);

				if (tg.enabled)
				{
					SetNewColor();
				}
				else
				{
					SetOrgColor();
				}

			}
		}
	}


	void SetNewColor()
	{
		if (mat != null)
			mat.color = NewColor;
	}

	void SetOrgColor()
	{
		if (mat != null)
			mat.color = OrgColor;
	}
}
