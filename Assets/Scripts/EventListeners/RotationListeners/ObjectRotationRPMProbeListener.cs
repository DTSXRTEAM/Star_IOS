using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationRPMProbeListener : CPEventListener {

	[SerializeField] private string ComponentName;

	private float CurrentAngle;
	private float TargetAngle;
	private float RPM;

	public bool stopped;

	private void Awake()
	{

		CurrentAngle = 0;
		TargetAngle = 0;
	}

	private void Start()
	{
		stopped = false;

		var probe = SimulatorInterface.ComponentManager.Instance.GetStoredComponent(ComponentName);
		if (probe != null)
		{
			CurrentAngle = ((SimulatorInterface.Probe)probe).data.value;
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
		if (parameter.GetType() == typeof(SimulatorInterface.Probe))
		{
			var probe = (SimulatorInterface.Probe)parameter;
			if (probe.name.ToLower() == ComponentName.ToLower())
			{
				RPM = probe.data.value;
			}
		}
	}
}
