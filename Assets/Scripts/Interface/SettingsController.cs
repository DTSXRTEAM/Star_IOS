using Cadpeople.Events;
using Network;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsController : CPEventListener {

	public GameObject SettingsButton;
	public GameObject SettingsDialog;
	public GameObject IpAdress;
	public GameObject Version;

	// When Image is found hide settings
	public override void OnEventRaised(object parameter)
	{
		SettingsButton.SetActive(false);
		SettingsDialog.SetActive(false);
	}

	public void ShowSettings()
	{
		var cfg = CPSettings.Instance.GetConfig();

		IpAdress.GetComponent<TMP_InputField>().text = cfg.serverIp;

		var ee = Version.GetComponent<TextMeshProUGUI>();
		Version.GetComponent<TextMeshProUGUI>().text = "Version: " + Application.version;

		SettingsDialog.SetActive(true);
	}

	public void OnOK()
	{
		// save ip
		var cfg = CPSettings.Instance.GetConfig();
		cfg.serverIp = IpAdress.GetComponent<TMP_InputField>().text;
		CPSettings.Instance.SaveConfig(cfg);

		// close
		SettingsDialog.SetActive(false);
	}
}
