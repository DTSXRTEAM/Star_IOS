using Cadpeople.Events;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenInteraction : MonoBehaviour {

	public GameEvent showInteractionMenuEvent;
	public GameEvent highlightComponentEvent;

	// Update is called once per frame
	void Update ()
	{
		// Handle screen touches.
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
			{
				Debug.Log("Over UI");
				return;
			}

			if(touch.phase == TouchPhase.Began)
			{
				var ray = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					var clickScript = hit.collider.gameObject.GetComponent<Interactable>();
					if (clickScript != null)
					{
						clickScript.Click();
					}
				}
				else if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
				{
					highlightComponentEvent?.Raise("");
					showInteractionMenuEvent?.Raise(new List<MenuAction>());
				}
			}
		}
		else if(Input.GetMouseButtonDown(0)) // mouse for testing
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				Debug.Log("Over UI");
				return;
			}

			var cc = GameObject.FindGameObjectWithTag("EditorCamera");
			if (cc != null)
			{
				var camera = cc.GetComponent<Camera>() as Camera;
				if (camera != null)
				{
					var ray = camera.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit))
					{
						var clickScript = hit.collider.gameObject.GetComponent<Interactable>();
						if (clickScript != null)
						{
							clickScript.Click();
						}
					}
					else if (!EventSystem.current.IsPointerOverGameObject())
					{
						highlightComponentEvent?.Raise("");
						showInteractionMenuEvent?.Raise(new List<MenuAction>());
					}
				}
			}
		}
	}
}

