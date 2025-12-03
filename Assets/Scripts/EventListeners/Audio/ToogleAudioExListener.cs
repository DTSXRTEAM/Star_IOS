using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayState { Stopped, Starting, Running, Stopping }

public class ToogleAudioExListener : CPEventListener
{
	[SerializeField] private string ComponentName;
	[SerializeField] private string PressureComponentName;
	[SerializeField] private AudioSource StartingAudioSource;
	[SerializeField] private AudioSource RunningAudioSource;
	[SerializeField] private AudioSource StoppingAudioSource;

	private PlayState CurrentState;
	private PlayState TargetState;


	private void Awake()
	{
		CurrentState = PlayState.Stopped;
		TargetState = PlayState.Stopped;
	}

	private void OnDestroy()
	{
		if (StartingAudioSource != null)
			StartingAudioSource.Stop();
		if (RunningAudioSource != null)
			RunningAudioSource.Stop();
		if (StoppingAudioSource != null)
			StoppingAudioSource.Stop();
	}

	private void Update()
	{
		// Transition
		if(CurrentState == PlayState.Stopped && TargetState == PlayState.Running)
		{
			Debug.Log("Starting pump");
			StartingAudioSource.Play();
			CurrentState = PlayState.Starting;
		}

		// Transition
		if (CurrentState == PlayState.Running && TargetState == PlayState.Stopped)
		{
			Debug.Log("stopping pump");
			if (RunningAudioSource != null)
				RunningAudioSource.Stop();
			StoppingAudioSource.Play();
			CurrentState = PlayState.Stopping;
		}

		// Update
		if(CurrentState == PlayState.Starting && !StartingAudioSource.isPlaying)
		{
			Debug.Log("Going from starting to running");
			if (RunningAudioSource != null)
				RunningAudioSource.Play();
			CurrentState = PlayState.Running;
		}

		if (CurrentState == PlayState.Stopping && !StoppingAudioSource.isPlaying)
		{
			Debug.Log("Going from stopping to stopped");
			CurrentState = PlayState.Stopped;
		}
	}

	private void SetFilter(int val)
	{
		if (RunningAudioSource != null && RunningAudioSource.isPlaying)
		{
			var Lowpass_CutoffFreq = 20000 - (val * 225);
			if (Lowpass_CutoffFreq < 0)
				Lowpass_CutoffFreq = 2000;
			RunningAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("Lowpass_CutoffFreq", Lowpass_CutoffFreq);
		}
	}



	public override void OnEventRaised(object par)
	{
		// Is it toggle component
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var toggle = (SimulatorInterface.Toggle)par;

			// Is it the right toggle name id
			if (toggle.name.ToLower() == ComponentName.ToLower())
			{
				TargetState = toggle.enabled == true ? PlayState.Running : PlayState.Stopped;
			}
		}

		// Is it pressure/probe component
		if (par.GetType() == typeof(SimulatorInterface.Probe))
		{
			var probe = (SimulatorInterface.Probe)par;

			// Is it the right toggle name id
			if (probe.name.ToLower() == PressureComponentName.ToLower())
			{
				SetFilter((int)probe.data.value);
			}
		}
	}
}
