using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolWatcherToolBoxListener : CPEventListener
{
	[SerializeField] private GameEvent SendObjectToServer;
	[SerializeField] private string ToolName;
	[SerializeField] private string ToggleComponentName;

	[SerializeField] private bool SetIfTrue;
	[SerializeField] private bool SetIfFalse;

	public override void OnEventRaised(object par)
	{
		// Is it toolbox component
		if (par.GetType() == typeof(SimulatorInterface.Toolbox))
		{
			var toolbox = (SimulatorInterface.Toolbox)par;

			// Is it the ri
			if (toolbox.tool.ToLower() == ToolName.ToLower())
			{
				if (SetIfTrue)
				{ 
					var d = new SendObjectdata
					{
						name = ToggleComponentName,
						data = new { enabled = true }
					};
					SendObjectToServer?.Raise(d);
				}
			}
			else
			{
				if (SetIfFalse)
				{
					var d = new SendObjectdata
					{
						name = ToggleComponentName,
						data = new { enabled = false }
					};
					SendObjectToServer?.Raise(d);
				}
			}
		}
	}
}
