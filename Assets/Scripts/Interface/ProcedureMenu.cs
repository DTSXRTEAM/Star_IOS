using Cadpeople.Events;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Newtonsoft.Json;
using TMPro;

public class ProcedureMenu : CPEventListener {

	public GameEvent loadScenarioEvent;
	public GameEvent scanForNewModelEvent;

	public GameObject MenuItemPrefab;
	public MainComponentGroup mainComponentGroup;
	public GameObject MainComponentName;
	public GameObject ProcedureList;
	private Transform Background;
	public GameObject HomeButton;
	public GameObject ScaleButton;
	public GameObject ActivityInfo;

	private MainComponent currentMC  = null;
	private CanvasGroup canvasGroup = null;

	public void Awake()
	{
		Background = transform.GetChild(0);
		canvasGroup = Background.GetComponent<CanvasGroup>();
	}

	public override void OnEventRaised(object parameter)
	{
		// hide menu
		showMenu(false);

		var model = (string)parameter;
		currentMC = mainComponentGroup.Components.FirstOrDefault(e => e.QRModelKey == model);

		// Set componentName header
		var componentName = MainComponentName.GetComponent<TextMeshProUGUI>();
		if(componentName != null)
		{
			componentName.text = currentMC.name;
		}

		// Clear menu
		foreach (Transform child in ProcedureList.transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		for (int i = 0; i < currentMC.Procedures.Length; i++)
		{
			

			var mi = Instantiate(MenuItemPrefab);

			foreach (Transform t in mi.transform)
			{
				if (t.name == "Header")
				{
					var header = t.GetComponent<TextMeshProUGUI>();
					if (header != null)
					{
						header.text = currentMC.Procedures[i].Header;
					}
				}
				else if (t.name == "Description")
				{
					var desc = t.GetComponent<TextMeshProUGUI>();
					if (desc != null)
					{
						desc.text = currentMC.Procedures[i].Description;
					}
				}
			}

			if (currentMC.Procedures[i].IsActive)
			{ 
				// Set clickHandler
				var xx = i; // fix for strange bug in unity, causing i beeing reference and thereby same value in all
				mi.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(xx));
			}

			// Add to parent 
			mi.transform.SetParent(ProcedureList.transform, false);
		};

		// show menu
		showMenu(true);
	}

	public void ButtonClicked(int menuIndex)
	{
		// Send actions
		if(currentMC != null && currentMC.Procedures != null && currentMC.Procedures.Length >= menuIndex)
		{
			loadScenarioEvent?.Raise(currentMC.Procedures[menuIndex].SimulatorScenario);
		}

		// hide menu
		showMenu(false);
	}

	public void HideMenu()
    {
        showMenu(false);
    }

	public void StartScan()
	{
		HideMenu();
		scanForNewModelEvent?.Raise("");
	}

	private void showMenu(bool show)
	{
		if(show)
		{
			// Show menu
			Background.gameObject.SetActive(true);
			canvasGroup.alpha = 1;

			HomeButton.SetActive(false);
			ScaleButton.SetActive(false);
			ActivityInfo.SetActive(false);
		}
		else
		{
			// Hide menu
			Background.gameObject.SetActive(false);
			canvasGroup.alpha = 0;

			HomeButton.SetActive(true);
			ScaleButton.SetActive(true);
			ActivityInfo.SetActive(true);
		}
	}

}
