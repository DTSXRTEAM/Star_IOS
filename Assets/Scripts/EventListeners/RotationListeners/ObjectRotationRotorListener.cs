using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationRotorListener : CPEventListener {

	[SerializeField] private GameEvent SendObjectToServer;
	[SerializeField] private string ComponentName;

	[SerializeField] private float CurrentAngle;
	[SerializeField] private float TargetAngle;
	[SerializeField] private float RPM;

	private bool stopped;

	private void Awake()
	{

		CurrentAngle = 0;
		TargetAngle = 0;
	}

	private void Start()
	{
		stopped = false;

		var rotor = SimulatorInterface.ComponentManager.Instance.GetStoredComponent(ComponentName);
		if (rotor != null)
		{
			CurrentAngle = ((SimulatorInterface.Rotor)rotor).rotation;
			TargetAngle = CurrentAngle;
		}
	}

	private void FixedUpdate()
	{
		TargetAngle += RPM * 6f * Time.deltaTime;


		if (Mathf.Abs(CurrentAngle - TargetAngle) > 300)
		{
			CurrentAngle = TargetAngle;
		}
		else
		{
			CurrentAngle = Mathf.Lerp(CurrentAngle, TargetAngle, Time.deltaTime * 5);
		}

//		Debug.Log("Current Angle : " + CurrentAngle);
		transform.localEulerAngles = new Vector3(0, -CurrentAngle, 0);

		if (Input.GetKeyDown(KeyCode.B))
		{
			var d = new SendObjectdata
			{
				name = "brake",
				data = new { enabled = stopped }
			};
			SendObjectToServer?.Raise(d);

			stopped = stopped == true ? false : true;
		}

	}

	public override void OnEventRaised(object parameter)
	{
		if (parameter.GetType() == typeof(SimulatorInterface.Rotor))
		{
			var rotor = (SimulatorInterface.Rotor)parameter;

			if (rotor.name.ToLower() == ComponentName.ToLower())
			{
//				Debug.Log("Updated Taget Angle from: " + CurrentAngle + " to: " + rotor.rotation);
				TargetAngle = rotor.rotation;

//				Debug.Log("RPM from : " + RPM + " to: " + rotor.rpm);
				RPM = rotor.rpm;
			}
		}
	}
}
