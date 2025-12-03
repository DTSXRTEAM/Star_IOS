using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationProbeListener : CPEventListener {

	[SerializeField] private string ComponentName;

	[SerializeField] private float CurrentAngle;
	[SerializeField] private float TargetAngle;
	[SerializeField] private bool IsRotating;

	private void Awake()
	{
		CurrentAngle = 0;
	}


	private void Update()
	{
		if (IsRotating)
		{
			if (Math.Abs(CurrentAngle - TargetAngle) > 0.05f)
			{
				CurrentAngle = Mathf.Lerp(CurrentAngle, TargetAngle, Time.deltaTime * 5);
				transform.localEulerAngles = new Vector3(0, CurrentAngle,0);
			}
			else
			{
				transform.localEulerAngles = new Vector3(0, TargetAngle, 0);
				IsRotating = false;
			}
		}
	}

	public override void OnEventRaised(object parameter)
	{
		if (parameter.GetType() == typeof(SimulatorInterface.Probe))
		{
			var probe = (SimulatorInterface.Probe)parameter;

			if (probe.name.ToLower() == ComponentName.ToLower())
			{
				TargetAngle = -(probe.data.value * 0.6428571f);
				IsRotating = true;
			}
		}
	}
}
