using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationToggleListener : CPEventListener {

	[SerializeField] private string ComponentName;

	[SerializeField] private float TrueAngle = 45;
	[SerializeField] private float FalseAngle = 0;

	private bool IsRotating;
	private float CurrentAngle;
	private float TargetAngle;



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
				CurrentAngle = Mathf.Lerp(CurrentAngle, TargetAngle, Time.deltaTime * 8);
				transform.localEulerAngles = new Vector3(CurrentAngle, 0, 0);
			}
			else
			{
				transform.localEulerAngles = new Vector3(TargetAngle, 0, 0);
				IsRotating = false;
			}
		}
	}

	public override void OnEventRaised(object parameter)
	{
		if (parameter.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var tg = (SimulatorInterface.Toggle)parameter;

			if (tg.name.ToLower() == ComponentName.ToLower())
			{
				if(tg.enabled)
				{
					TargetAngle = TrueAngle;
				}
				else
				{
					TargetAngle = FalseAngle;
				}
				IsRotating = true;
			}
		}
	}
}
