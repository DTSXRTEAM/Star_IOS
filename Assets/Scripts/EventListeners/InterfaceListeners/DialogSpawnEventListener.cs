using Cadpeople.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSpawnInfo
{
	public int errorNo;

	public DialogSpawnInfo(int errorNo)
	{
		this.errorNo = errorNo;
	}
}

public class DialogSpawnEventListener : CPEventListener {

	public GameObject DefaultDialogPrefab;

	private GameObject CurrentDialog = null;

	public void Awake()
	{
	}
		
	public void CloseCurrentDialog()
	{
		foreach (Transform child in transform)
		{
			var baseInfoDialogScript = child.GetComponent<BaseInfoDialog>();
			if (baseInfoDialogScript != null)
			{
				Destroy(baseInfoDialogScript.gameObject,200);
			}
		}
		CurrentDialog = null;
	}

	public override void OnEventRaised(object par)
	{
		var si = (DialogSpawnInfo)par;

		if (si.errorNo > 0)
		{
			Debug.Log("===== Show dialog no: " + si.errorNo + " =====");

			if (AllreadyShowing(si.errorNo))
				return;

			var entry = DialogTable.Instance.GetTableEntry(si.errorNo);

			CurrentDialog = Instantiate(DefaultDialogPrefab);

			// Header
			var topbar  = CurrentDialog.transform.Find("Topbar").gameObject;
			var headerText = topbar.GetComponentInChildren<TextMeshProUGUI>();
			headerText.text = entry.HeaderText;

			// Body
			var content = CurrentDialog.transform.Find("ContentArea").gameObject;
			var bodyText = content.GetComponentInChildren<TextMeshProUGUI>();
			bodyText.text = entry.BodyText;
			LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());

			// Buttons
			var button = content.GetComponentInChildren<Button>();
			button.GetComponentInChildren<Text>().text = entry.Buttons[0];
			LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());

			var buttonScript = CurrentDialog.GetComponent<BaseInfoDialog>();
			if(buttonScript != null)
			{
				buttonScript.ErrorNo = si.errorNo;
			}

			CurrentDialog.transform.SetParent(transform, false);
			CurrentDialog.SetActive(true);

			StartCoroutine(DelayRecalculate(CurrentDialog));
		}
	}

	private bool AllreadyShowing(int error)
	{
		foreach (Transform child in transform)
		{
			var buttonScript = child.GetComponent<BaseInfoDialog>();
			if (buttonScript != null && buttonScript.ErrorNo == error)
			{
				return true;
			}
		}
		return false;
	}

	private IEnumerator DelayRecalculate(GameObject panel)
	{
		yield return new WaitForEndOfFrame();

		var topbar = panel.transform.Find("Topbar").gameObject;
		LayoutRebuilder.ForceRebuildLayoutImmediate(topbar.GetComponent<RectTransform>());

		var content = panel.transform.Find("ContentArea").gameObject;
		LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());

		var cc = panel.GetComponent<RectTransform>();
		LayoutRebuilder.ForceRebuildLayoutImmediate(cc);

	}
}
