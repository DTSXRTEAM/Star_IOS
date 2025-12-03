using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUpdateFloorListener : CPEventListener {

	public string ComponentName;
	public string Extension;
	private TextMeshProUGUI TextLabel;

	private void Awake()
	{
		TextLabel = GetComponent<TextMeshProUGUI>();
	}

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Floor))
		{
			var floor = (SimulatorInterface.Floor)par;

			if (floor.name.ToLower() == ComponentName.ToLower())
			{
				TextLabel.text = floor.value + Extension; 
			}
		}
	}
}
