using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComponentPickerManager : CPEventListener
{
	[SerializeField]
	private MainComponentGroup mainComponentGroup;
	[SerializeField]
	private Transform list;
	[SerializeField]
	private GameObject componentButtonPrefab;
	[SerializeField]
	private GameObject window;
	[SerializeField]
	private GameEvent showProcedureListEvent;
	[SerializeField]
	private GameEvent currentComponentUpdatedEvent;
	[SerializeField]
	private GameEvent scanForNewModelEvent;

	public override void OnEventRaised(object parameter)
	{

		// Clear menu
		for (int i = list.childCount - 1; i >= 0; i--)
		{
			Destroy(list.GetChild(i).gameObject);
		}

		for (int i = 0; i < mainComponentGroup.Components.Count; i++)
		{
			var comp = mainComponentGroup.Components[i];
			// Instantiate and set title text
			var newMenuButton = Instantiate(componentButtonPrefab, list);

			var textInButtonList = newMenuButton.GetComponentsInChildren<TextMeshProUGUI>();
			textInButtonList[0].text = comp.ComponentName;
			textInButtonList[1].text = comp.Description;

			newMenuButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { SelectComponent(comp.QRModelKey); });

		}

	}

	public void StartScan()
	{
		scanForNewModelEvent.Raise(null);
		window.SetActive(false);
	}

	public void SelectComponent(string componentName)
	{
		currentComponentUpdatedEvent.Raise(componentName);
		showProcedureListEvent.Raise(componentName);
		window.SetActive(false);
	}


}
