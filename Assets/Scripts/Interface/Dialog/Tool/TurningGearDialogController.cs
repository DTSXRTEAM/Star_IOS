using Cadpeople.Events;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurningGearDialogController : ToolDialogController
{
	[SerializeField] private string DirectionComponentName;
	[SerializeField] private Image ActivateImage;
	[SerializeField] private Image ActivatePressedImage;
	[SerializeField] private Image UpImage;
	[SerializeField] private Image UpPressedImage;
	[SerializeField] private Image DownImage;
	[SerializeField] private Image DownPressedImage;

	private bool disabled;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake();




		//var enumerator = ComponentManager.Instance.GetStoredComponent("locking-tab");
		//if (enumerator != null)
		//{
		//	if( ((SimulatorInterface.Enumerator)enumerator).index == 0)
		//	{
		//		disabled = false;
		//	}
		//	else
		//	{
		//		disabled = true;
		//	}
		//}

		// Disable interaction ?
		//var tg = ComponentManager.Instance.GetStoredComponent("pin-stuck");
		//if (tg != null && ObjectToActivate != null)
		//{
		//	ObjectToActivate.SetActive(((SimulatorInterface.Toggle)tg).enabled);
		//}
		//currentLeverState = 0;
	}

	public override void OnEventRaised(object par)
	{
		// Hold to run
		if (par.GetType() == typeof(SimulatorInterface.Toggle))
		{
			var tg = (SimulatorInterface.Toggle)par;

			if (tg.name.ToLower() == ComponentName.ToLower())
			{
				if(tg.enabled)
				{
					ActivatePressedImage.enabled = true;
				}
				else
				{
					ActivatePressedImage.enabled = false;
				}
			}
		}

		// Turning Gear direction 
		if (par.GetType() == typeof(SimulatorInterface.Enumerator))
		{
			var Enumerator = (SimulatorInterface.Enumerator)par;
			if (Enumerator.name.ToLower() == DirectionComponentName.ToLower())
			{
				if (Enumerator.index == 1)
				{
					Debug.Log("-> Up");
					UpPressedImage.enabled = true;
					DownPressedImage.enabled = false;
				}
				else if (Enumerator.index == 2)
				{
					Debug.Log("-> Down");
					DownPressedImage.enabled = true;
					UpPressedImage.enabled = false;
				}
				else
				{
					Debug.Log("-> Nothing");
					UpPressedImage.enabled = false;
					DownPressedImage.enabled = false;
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
	}

	public void OnOk()
	{
		GetComponent<BaseDialogTool>().OnClose();
	}

	public void OnActivatePressed()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { enabled = true } });
	}

	public void OnActivateReleased()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = ComponentName, data = new { enabled = false } });
	}

	public void OnUpPressed()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = DirectionComponentName, data = new { index = 1 } });
	}

	public void OnUpReleased()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = DirectionComponentName, data = new { index = 0 } });
	}
	public void OnDownPressed()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = DirectionComponentName, data = new { index = 2 } });
	}

	public void OnDownReleased()
	{
		sendOjectToServerEvent?.Raise(new SendObjectdata { name = DirectionComponentName, data = new { index = 0 } });
	}
}
