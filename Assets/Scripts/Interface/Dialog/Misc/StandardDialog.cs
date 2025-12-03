using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardDialog : MonoBehaviour
{
	public int ErrorNo { get; set; }

	public void OnClose()
	{
		gameObject.transform.SetParent(null, false);
		Destroy(gameObject);
	}
}
