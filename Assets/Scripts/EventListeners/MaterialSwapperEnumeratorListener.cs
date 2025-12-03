using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapperEnumeratorListener : CPEventListener
{
	public string ComponentName;
	[SerializeField] private List<Material> Materials;
	[SerializeField] private int MaterialNumber = 0;

	private Material mat = null;

	private HighLightComponent hightlightComponentScript = null;

	private void Awake()
	{
		if (GetComponent<Renderer>().materials.Length > MaterialNumber)
		{
			GetComponent<Renderer>().materials[MaterialNumber] = Materials[0];
		}

		hightlightComponentScript = GetComponent<HighLightComponent>();
	}

	public override void OnEventRaised(object par)
	{
		// Is it toggle component
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var en = (SimulatorInterface.Enumerator)par;

			// Is it the right toggle name id
			if (en.name.ToLower() == ComponentName.ToLower())
			{
				if (en.index < Materials.Count)
				{
					var mats = GetComponent<Renderer>().materials;
					mats[MaterialNumber] = Materials[en.index];
					GetComponent<Renderer>().materials = mats;

					// Set org material in highlightComponent
					hightlightComponentScript.UpdateOrgMaterial(Materials[en.index], MaterialNumber);
				}
			}
		}
	}
}
