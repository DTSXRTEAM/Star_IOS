using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CPConfig
{
	public string serverIp;
}


public class CPSettings : Singleton<CPSettings>
{

	public CPConfig GetConfig()
	{
		CPConfig cfg = new CPConfig
		{
			serverIp = PlayerPrefs.GetString("serverIp")
		};

		//if(cfg.serverIp.Length < 15)
		//{
		//	cfg.serverIp = "192.168.1.245";
		//	SaveConfig(cfg);
		//}
		return cfg;
	}

	public void SaveConfig(CPConfig cfg)
	{
		PlayerPrefs.SetString("serverIp", cfg.serverIp);
		PlayerPrefs.Save();
	}
}
