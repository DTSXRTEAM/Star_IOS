using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanTextController : MonoBehaviour
{
	public GameObject ScanTextPrefab;

	private GameObject scanText = null;

	private void Start()
	{
		if(ScanTextPrefab != null)
		{
			scanText = Instantiate(ScanTextPrefab);
			scanText.transform.SetParent(transform, false);
			//			scanText.SetActive(false);
		}
	}

	public void ShowText()
	{
		scanText.SetActive(true);
	}

	public void HideText()
	{
		scanText.SetActive(false);
	}
}
