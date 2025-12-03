using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapperToggleListener : CPEventListener
{
	public string ComponentName;
	[SerializeField] private Material ToggleTrueMaterial;
	[SerializeField] private Material ToggleFalseMaterial;
//	[SerializeField] private int MaterialNumber = 0;

	private Renderer render = null;

	private void Awake()
	{
		render = GetComponent<Renderer>();
	}

	public override void OnEventRaised(object par)
	{
		// Is it toggle component
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var tg = (SimulatorInterface.Toggle)par;

			// Is it the right toggle name id
			if (tg.name.ToLower() == ComponentName.ToLower())
			{
				if(tg.enabled == true)
				{
					var mats = render.materials;
					mats[0] = ToggleTrueMaterial;
					render.materials = mats;
				}
				else
				{
					var mats = render.materials;
					mats[0] = ToggleFalseMaterial;
					render.materials = mats;
				}
			}
		}
	}
}
