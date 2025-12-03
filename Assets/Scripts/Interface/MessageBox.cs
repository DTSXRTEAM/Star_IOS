using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBox : MonoBehaviour {

	public GameEvent dialogButtonClickedEvent;

	public void OnClick()
	{
		var text = GetComponent<TextMeshProUGUI>().text;
		dialogButtonClickedEvent?.Raise(text);

		var xx = transform.parent.gameObject;

		Destroy(transform.parent.gameObject, 1);
	}
}
