using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorSwapper : MonoBehaviour
{
	[SerializeField] private Color NewColor;
	[SerializeField] private int MaterialNumber = 0;

	private Color OrgColor;
	private Material mat = null;

	private void Start()
	{
		if (GetComponent<Renderer>().materials.Length > MaterialNumber)
		{
			mat = GetComponent<Renderer>().materials[MaterialNumber];
			OrgColor = mat.color;
		}
	}

	public void SetNewColor()
	{
		if(mat != null)
			mat.color = NewColor;
	}

	public void SetOrgColor()
	{
		if (mat != null)
			mat.color = OrgColor;
	}
}
