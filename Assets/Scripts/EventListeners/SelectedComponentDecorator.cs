using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedComponentDecorator : CPEventListener
{
	[SerializeField] private string ComponentName;
	[SerializeField] private int MaterialNumber = 0;
	[SerializeField] private Material SelectedMaterial;

	private Material[] Materials = null;
	private Material OrgMaterial = null;

	private void Awake()
	{
		if (GetComponent<Renderer>().materials.Length > MaterialNumber)
		{
			Materials = GetComponent<Renderer>().materials;
			OrgMaterial = Materials[MaterialNumber];
		}
	}

	public override void OnEventRaised(object par)
	{
		string s = par as string;
		if(!string.IsNullOrWhiteSpace(s) && s.ToLower() == ComponentName.ToLower())
		{
			Materials[MaterialNumber] = SelectedMaterial;
		}
		else
		{
			Materials[MaterialNumber] = OrgMaterial;
		}
	}
}
