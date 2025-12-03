using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(ToolSelector))]
public class ToolButtonListener : CPEventListener
{
	private Button button;
	private string toolName;
	public Sprite unselected;
	public Sprite selected;

	public void Awake()
	{
		button = GetComponent<Button>();
		toolName = GetComponent<ToolSelector>().toolName;
	}

	public override void OnEventRaised(object parameter)
	{
		var component = (SimulatorInterface.Component)parameter;
		if (component.GetType() == typeof(SimulatorInterface.Toolbox))
		{
			var toolbox = (SimulatorInterface.Toolbox)component;
			SetSelected(toolbox.tool);
		}
	}

	public void SetSelected(string toolboxTool)
	{
		if (toolboxTool == toolName)
		{
			button.image.overrideSprite = selected;
		}
		else
		{
			button.image.overrideSprite = unselected;
		}

	}
}
