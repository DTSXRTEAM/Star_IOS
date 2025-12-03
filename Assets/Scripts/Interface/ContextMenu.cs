using Cadpeople.Events;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : CPEventListener {

	public GameEvent sendObjectEvent;
	public GameEvent componentHighlightEvent;
	public GameObject MenuItemPrefab;

	private List<MenuAction> menulist = null;
	private CanvasGroup canvasGroup = null;

	public void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public override void OnEventRaised(object parameter)
	{
		// hide menu
		showMenu(false);

		menulist = (List<MenuAction>)parameter;


		// Clear menu
		foreach (Transform child in transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		if (menulist.Count == 0)
			return;

		for (int i = 0; i < menulist.Count;i++)
		{
			var mi = Instantiate(MenuItemPrefab);

			// Set Text
			var textList = mi.GetComponentsInChildren<Text>();
			if(textList != null && textList.Length > 0)
			{
				textList[0].text = menulist[i].title;
			}

			// Set clickHandler
			var xx = i;	// fix for strange bug in unity, causing i beeing reference and thereby same value in all
			mi.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(xx));

			// Add to parent 
			mi.transform.SetParent(transform, false);
		};

		// show menu
		showMenu(true);
	}

	public void ButtonClicked(int menuIndex)
	{
		// Send actions
		if(menulist != null && menulist.Count >= menuIndex)
		{
			menulist[menuIndex].actionsToSend.ForEach(ac =>
			{
				sendObjectEvent?.Raise(ac);
			});
		}

		// remove hightlight
		componentHighlightEvent?.Raise("");

		// hide menu
		showMenu(false);
	}

	public void ComponentUpdated(object par)
	{
		//if (menulist != null)
		//{
		//	// Remove highlight
		//	componentHighlightEvent?.Raise("");

		//	// Hide context menu
		//	showMenu(false);

		//	menulist = null;
		//}
	}

	private void showMenu(bool show)
	{
		if(show)
		{
			// Show menu
			canvasGroup.alpha = 1;
		}
		else
		{
			// Hide menu
			canvasGroup.alpha = 0;
		}
	}

}
