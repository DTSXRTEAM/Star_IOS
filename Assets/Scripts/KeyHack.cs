using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHack : MonoBehaviour
{
	[SerializeField] private GameEvent sendOjectToServerEvent;

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Z))
		{
			sendOjectToServerEvent?.Raise(new SendObjectdata { name = "tap", data = new { index = 2 } });
		}

		if (Input.GetKeyDown(KeyCode.X))
		{
			sendOjectToServerEvent?.Raise(new SendObjectdata { name = "tap", data = new { index = 1 } });
		}
	}
}
