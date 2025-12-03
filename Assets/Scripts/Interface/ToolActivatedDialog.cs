using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Network;

[Serializable]
public class ScenarioToolDialog
{
	public string Scenario;
	public GameObject DialogPrefab;
}

public class ToolActivatedDialog : CPEventListener
{
	public string ToolNameToActivate;
//	public GameObject DialogPrefab;
//	private string LoadedScenario = "";


	[SerializeField] public List<ScenarioToolDialog> ScenarioToolDialogList;

	public override void OnEventRaised(object par)
	{
		if (par.GetType() == typeof(SimulatorInterface.Toolbox))
		{
			var toolbox = (SimulatorInterface.Toolbox)par;

			if(toolbox.tool.ToLower() == ToolNameToActivate.ToLower())
			{
				var ss = ScenarioToolDialogList.FirstOrDefault(e => e.Scenario == NetworkManager.Instance.currentScenario);
				if(ss != null)
				{
					var dialog = Instantiate(ss.DialogPrefab);

					// Add to parent 
					dialog.transform.SetParent(transform, false);
				}
			}
		}
	}
}
