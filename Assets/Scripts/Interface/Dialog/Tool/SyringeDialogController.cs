using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SyringeDialogController : ToolDialogController
{
	[SerializeField] private string valveComponentName;
	[SerializeField] private FixedJoystick joystick;
	[SerializeField] private  Image PosVacuumBarImage;
	[SerializeField] private Image NegVacuumBarImage;
	[SerializeField] private Transform SyringeValve;


	private int currentPlungerState;
	private float deadband = 0.2f;
	private int syringeValve = 0;

	private float targetValveAngle;
	private float currentValveAngle;
	private bool rotatingValve;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();

		var comp = ComponentManager.Instance.GetStoredComponent(valveComponentName);
		if(comp != null)
		{
			UpdateDialog(((SimulatorInterface.Enumerator)comp).index);
		}
		currentPlungerState = 0;
		currentValveAngle = 0;

		rotatingValve = true;
	}

	private void Update()
	{
		//vacuumText.text = string.Format("Vacuum: {0:0.0} bar", -joystick.Vertical);

		//if(joystick.Vertical > 0)
		//{
		//	PosVacuumBarImage.fillAmount = joystick.Vertical;
		//	NegVacuumBarImage.fillAmount = 0;	
		//}
		//else if(joystick.Vertical < 0)
		//{
		//	PosVacuumBarImage.fillAmount = 0;
		//	NegVacuumBarImage.fillAmount = -joystick.Vertical;
		//}
		//else
		//{
		//	PosVacuumBarImage.fillAmount = 0;
		//	NegVacuumBarImage.fillAmount = 0;
		//}
		//vacuumText.text = "Valve:" + syringeValve;

		if (joystick.Vertical < -deadband && currentPlungerState != 1)
		{
			currentPlungerState = 1;
			sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 1 } });
		}
		else if(joystick.Vertical > deadband && currentPlungerState != 2)
		{
			currentPlungerState = 2;
			sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 2 } });
		}
		else if (joystick.Vertical <= deadband && joystick.Vertical >= -deadband && currentPlungerState != 0)
		{
			currentPlungerState = 0;
			sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { index = 0 } });
		}

		// Syringe Valve
		if (rotatingValve)
		{
			if (Math.Abs(currentValveAngle - targetValveAngle) > 0.05f)
			{
				currentValveAngle = Mathf.Lerp(currentValveAngle, targetValveAngle, Time.deltaTime*8);
				SyringeValve.eulerAngles = new Vector3(0, 0, currentValveAngle);
			}
			else
			{
				SyringeValve.eulerAngles = new Vector3(0, 0, targetValveAngle);
				rotatingValve = false;
			}
		}
	}

	public override void OnEventRaised(object par)
	{
		// Syringe-plunger
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var Enumerator = (SimulatorInterface.Enumerator)par;
			if (Enumerator.name.ToLower() == ComponentName.ToLower())
			{
				if (Enumerator.index == 1)
				{
					PosVacuumBarImage.enabled = true;
					NegVacuumBarImage.enabled = false;
//					vacuumText.text = string.Format("Vacuum: {0:0.0} bar", 1.0);
				}
				else if (Enumerator.index == 2)
				{
					PosVacuumBarImage.enabled = false;
					NegVacuumBarImage.enabled = true;
//					vacuumText.text = string.Format("Vacuum: {0:0.0} bar", -1.0);
				}
				else
				{
					PosVacuumBarImage.enabled = false;
					NegVacuumBarImage.enabled = false;
//					vacuumText.text = string.Format("Vacuum: {0:0.0} bar", 0.0);
				}

				

				// UpdateDialog(formData.value);
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

		// Syringe-valve
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var enumerator = (SimulatorInterface.Enumerator)par;

			if (enumerator.name.ToLower() == valveComponentName)
			{
				syringeValve = enumerator.index;
				UpdateDialog(syringeValve);
			}
		}

	}

	public void RotateSyringeValve()
	{
		syringeValve++;
		if (syringeValve > 2)
			syringeValve = 0;
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = valveComponentName, data = new { index = syringeValve } });
	}

	protected void UpdateDialog(int value)
	{
		syringeValve = value;
		switch (value)
		{
			case 0:
				targetValveAngle = 90;
				rotatingValve = true;
				break;
			case 1:
				targetValveAngle = 270;
				rotatingValve = true;
				break;
			case 2:
				targetValveAngle = 180;
				rotatingValve = true;
				break;
		}
	}

	public void OnOk()
	{
		GetComponent<BaseDialogTool>().OnClose();
	}
}
