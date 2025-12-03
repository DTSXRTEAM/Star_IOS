using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

	public GameEvent buttonClickedEvent;

	public abstract void Click();
}
