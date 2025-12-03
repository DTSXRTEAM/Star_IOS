using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotorLockDialogController : ToolDialogController
{
	[SerializeField] private FixedJoystick joystick;
	[SerializeField] private Image LeftBarImage;
	[SerializeField] private Image RightBarImage;

	[SerializeField] private GameObject ObjectToActivate;

	private int currentLeverState;
	private float deadband = 0.2f;

	private float targetValveAngle;
	private float currentValveAngle;
	private bool rotatingValve;

	private bool disabled;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();

		var enumerator = ComponentManager.Instance.GetStoredComponent("locking-tab");
		if (enumerator != null)
		{
			if( ((SimulatorInterface.Enumerator)enumerator).index == 0)
			{
				disabled = false;
			}
			else
			{
				disabled = true;
			}
		}

		// Disable interaction ?
		var tg = ComponentManager.Instance.GetStoredComponent("pin-stuck");
		if (tg != null && ObjectToActivate != null)
		{
			ObjectToActivate.SetActive(((SimulatorInterface.Toggle)tg).enabled);
		}
		currentLeverState = 0;
	}

	private void Update()
	{
		if (disabled)
			return;

		if (joystick.Horizontal < -deadband && currentLeverState != 2)
		{
			currentLeverState = 2;
			sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 2 } });
		}
		else if(joystick.Horizontal > deadband && currentLeverState != 1)
		{
			currentLeverState = 1;
			sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 1 } });
		}
		else if (joystick.Horizontal <= deadband && joystick.Horizontal >= -deadband && currentLeverState != 0)
		{
			currentLeverState = 0;
			sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 0 } });
		}
	}


	public override void OnEventRaised(object par)
	{
		// RotorLock Lever
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var Enumerator = (SimulatorInterface.Enumerator)par;
			if (Enumerator.name.ToLower() == ComponentName.ToLower())
			{
				if (Enumerator.index == 1)
				{
					LeftBarImage.enabled = false;
					RightBarImage.enabled = true;
				}
				else if (Enumerator.index == 2)
				{
					LeftBarImage.enabled = true;
					RightBarImage.enabled = false;
				}
				else
				{
					LeftBarImage.enabled = false;
					RightBarImage.enabled = false;
				}
			}
		}

		// Close if tool is changed
		if (par.GetType() == typeof(SimulatorInterface.Toolbox))
		{
			var toolbox = (SimulatorInterface.Toolbox)par;

			if (toolbox.name.ToLower() != ToolName.ToLower())
			{
				Destroy(gameObject);
			}
		}

		// Hack
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var tg = (SimulatorInterface.Toggle)par;

			if (tg.name.ToLower() == "pin-stuck" && ObjectToActivate != null)
			{
				ObjectToActivate.SetActive(tg.enabled);
			}
		}

	}

	public void OnOk()
	{
		//var script = GetComponent<ObjectActivatorToolListener>();
		//if (script != null)
		//	script.enabled = false;

		GetComponent<BaseDialogTool>().OnClose();
	}
}
