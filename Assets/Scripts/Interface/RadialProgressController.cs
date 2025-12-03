using Cadpeople.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgressController : CPEventListener
{
	public Transform radialProgressBar;
	public Transform progressBar;
	public GameObject progressText;

	public override void OnEventRaised(object parameter)
	{
		var pd = parameter as ProgressData;

		// Show / Hide progress
		if(pd.progress <= 0 || pd.progress >= 100)
		{
			radialProgressBar.GetComponent<Image>().enabled = false;
			progressBar.GetComponent<Image>().enabled = false;
			progressText.SetActive(false);
		}
		else
		{
			radialProgressBar.GetComponent<Image>().enabled = true;
			progressBar.GetComponent<Image>().enabled = true;
			progressBar.GetComponent<Image>().fillAmount = pd.progress / 100.0f;

			if(!string.IsNullOrWhiteSpace(pd.progressText))
			{
				progressText.SetActive(true);
				progressText.GetComponent<TextMeshProUGUI>().text = pd.progressText;
			}
		}
	}
}
