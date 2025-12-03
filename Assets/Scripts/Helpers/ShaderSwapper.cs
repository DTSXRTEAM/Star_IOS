using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSwapper : CPEventListener
{
	public string ToolName;
	public Shader blinkShader;

	Renderer rend;
	Shader orgShader;

	void Awake()
	{
		rend = GetComponent<Renderer>();
		orgShader = rend.material.shader;
	}

	public override void OnEventRaised(object parameter)
	{
		var component = (SimulatorInterface.Component)parameter;
		if(component.GetType() == typeof(SimulatorInterface.Toolbox) && rend != null)
		{
			var toolbox = (SimulatorInterface.Toolbox)component;
			if(toolbox.tool == ToolName)
			{
				if (rend.material.shader == orgShader)
				{
					rend.material.shader = blinkShader;
				}
			}
			else
			{
				rend.material.shader = orgShader;
			}
		}
	}
}
