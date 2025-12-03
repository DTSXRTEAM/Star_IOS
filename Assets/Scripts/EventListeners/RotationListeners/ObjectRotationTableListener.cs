using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationTableListener : CPEventListener {

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

		var table = SimulatorInterface.ComponentManager.Instance.GetStoredComponent(ComponentName);
		if (table != null)
		{
			CurrentAngle = ((SimulatorInterface.Table)table).value;
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

		transform.localEulerAngles = new Vector3(0, -CurrentAngle, 0);
	}

	public override void OnEventRaised(object parameter)
	{
		if (parameter.GetType() == typeof(SimulatorInterface.Table))
		{
			var table = (SimulatorInterface.Table)parameter;
			if (table.name.ToLower() == ComponentName.ToLower())
			{
				RPM = table.value;
			}
		}
	}
}
