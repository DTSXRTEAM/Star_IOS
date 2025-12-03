using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManualPumpDialogController : ToolDialogController
{
	[SerializeField] private FixedJoystick joystick;
	[SerializeField] private Image ArrowUpImage;
	[SerializeField] private Image ArrowDownImage;


	private int currentLeverState;
	private float deadband = 0.2f;


	// Use this for initialization
	protected override void Awake()
	{
		base.Awake();

		currentLeverState = 0;
	}

	private void Update()
	{
		if (joystick.Vertical < -deadband && currentLeverState != 1)
		{
			currentLeverState = 1;
			sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { enabled = true } });
		}
		else if (joystick.Vertical > deadband && currentLeverState != 2)
		{
			currentLeverState = 2;
			sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { enabled = false } });
		}
		else if (joystick.Vertical <= deadband && joystick.Vertical >= -deadband && currentLeverState != 0)
		{
			currentLeverState = 0;
		//	sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 0 } });
		}
	}

	public override void OnEventRaised(object par)
	{
		// RotorLock Lever
		//if (par.GetType() == typeof(SimulatorInterface.Toggle))
		//{
		//	var tg = (SimulatorInterface.Toggle)par;
		//	if (tg.name.ToLower() == ComponentName.ToLower())
		//	{
		//		if (tg.enabled == true)
		//		{
		//			ArrowUpImage.enabled = true;
		//			ArrowDownImage.enabled = false;
		//		}
		//		else if (tg.enabled == false)
		//		{
		//			ArrowUpImage.enabled = false;
		//			ArrowDownImage.enabled = true;
		//		}
		//	}
		//}

		// Close if tool is changed
		if (par.GetType() == typeof(SimulatorInterface.Toolbox))
		{
			var toolbox = (SimulatorInterface.Toolbox)par;

			if (toolbox.name.ToLower() != ToolName.ToLower())
			{
				Destroy(gameObject);
			}
		}
	}

	public void OnOk()
	{
		GetComponent<BaseDialogTool>().OnClose();
	}
}
