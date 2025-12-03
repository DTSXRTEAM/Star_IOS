using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInfoDialog : MonoBehaviour
{
	[SerializeField] public GameEvent sendOjectToServerEvent;

	public int ErrorNo { get; set; }

	public void OnClose()
	{
		var evt = new SendObjectdata
		{
			name = "error" + ErrorNo.ToString("D2") + "-reset",
			data = new { execute = true }

		};

		sendOjectToServerEvent?.Raise(evt);

		gameObject.transform.SetParent(null, false);
		Destroy(gameObject);
	}
}
