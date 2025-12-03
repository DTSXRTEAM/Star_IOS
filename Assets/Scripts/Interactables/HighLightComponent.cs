using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightComponent : CPEventListener
{
	public string HighlightName;
	[SerializeField] private Material SelectedMaterial;
	[SerializeField] private int[] MaterialNumbers = new int[]{0};

	private Material[] OrgMaterials;

	private Material mat = null;

	private void Awake()
	{
		OrgMaterials = GetComponent<Renderer>().materials;
	}

	public override void OnEventRaised(object par)
	{
		var name = par as string;
		//var numbers = MaterialNumbers.Split(';');


		// Is it the right toggle name id
		if (name.ToLower() == HighlightName.ToLower())
		{
			var mats = GetComponent<Renderer>().materials;
			for(int i = 0; i < MaterialNumbers.Length;i++)
			{
				mats[MaterialNumbers[i]] = SelectedMaterial;
			}
			GetComponent<Renderer>().materials = mats;
		}
		else
		{
			var mats = GetComponent<Renderer>().materials;

			for (int i = 0; i < MaterialNumbers.Length; i++)
			{
				mats[MaterialNumbers[i]] = OrgMaterials[MaterialNumbers[i]];
			}
			GetComponent<Renderer>().materials = mats;
		}
	}

	public void UpdateOrgMaterial(Material mat, int number)
	{
		OrgMaterials[number] = mat;
	}
}
