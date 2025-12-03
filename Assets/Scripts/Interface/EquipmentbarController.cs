using Cadpeople.Events;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using SimulatorInterface;

[Serializable]
public class Equipment
{
	public string ComponentName;
	public GameObject EquipmentPrefab;
}

public class EquipmentbarController : CPEventListener {

	public Equipment[] EquipmentList;

	public override void OnEventRaised(object par)
	{
		var component = (SimulatorInterface.Component)par;
		if (component.GetType() == typeof(SimulatorInterface.Toolbox))
		{
			var toolbox = (SimulatorInterface.Toolbox)component;

			// Get complete toolbox
			var entireToolbox = ComponentManager.Instance.GetToolbox();

			if (transform.childCount == 0)
			{
				for (int i = 0; i < entireToolbox.tools.Length; i++)
				{
					if (entireToolbox.tools[i].enabled)
					{
						// Is tools availabel in this scenario
						var toolname = toolbox.tools.FirstOrDefault(e => e == entireToolbox.tools[i].id);
						if (toolname != null)
						{
							// Get from equipment list
							var equipment = EquipmentList.FirstOrDefault(e => e.ComponentName == toolname);
							if (equipment != null)
							{
								// create prefab
								var mi = Instantiate(equipment.EquipmentPrefab);

								// Set tool name
								mi.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = entireToolbox.tools[i].name;

								// Toggle selected
								var script = mi.transform.Find("Button").GetComponent<ToolButtonListener>();
								if (script != null)
								{
									script.SetSelected(toolbox.tool);
								}

								// Add to parent
								mi.transform.SetParent(transform, false);
							}
						}
					}
				}
			}



			//if(transform.childCount == 0)
			//{
			//	for(int i = 0; i < toolbox.tools.Length;i++)
			//	{
			//		var equipment = EquipmentList.FirstOrDefault(e => e.ComponentName == toolbox.tools[i]);
			//		if(equipment != null)
			//		{
			//			// Get complete tool
			//			var ct = entireToolbox.tools.FirstOrDefault(e => e.id == equipment.ComponentName);

			//			if (ct != null && ct.enabled)
			//			{
			//				// create prefab
			//				var mi = Instantiate(equipment.EquipmentPrefab);

			//				// Set tool name
			//				mi.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = ct.name;

			//				// Toggle selected
			//				var script = mi.transform.Find("Button").GetComponent<ToolButtonListener>();
			//				if(script != null)
			//				{
			//					script.SetSelected(toolbox.tool);
			//				}

			//				// Add to parent
			//				mi.transform.SetParent(transform, false);
			//			}
			//		}

			//	}
			//}

		}
	}

	public void Clear()
	{
		// New toolbox received, renew
		if (transform.childCount > 0)
		{

			for (int i = transform.childCount-1; i >= 0; i--)
			{
				var child = transform.GetChild(i);
				child.SetParent(null);
				Destroy(child.gameObject);
			}
		}
	}
}
