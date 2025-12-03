using Cadpeople.Events;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SimulatorInterface;

public class ModelSpawnerListener : CPEventListener
{
	[Serializable]
	public class ModelPrefab
	{
		public string modelName;
		public GameObject prefab;
	}
	public ModelPrefab[] spawnableModels;

	private GameObject spawnedPrefab = null;

	private string modelName = "";


	public void Awake()
	{
	//	anim = GetComponent<Animator>();
	}

	public override void OnEventRaised(object par)
	{
		var setup = (ComponentSetup)par;
		//var mp = spawnableModels.FirstOrDefault(e => e.modelName == setup.componentName);

		// Set scale
		if (setup.scale > 0)
		{
			var scale = transform.localScale;
			scale.Set(setup.scale, setup.scale, setup.scale);
			transform.localScale = scale;
		}

		modelName = setup.componentName;

		// Instanciate prefab and add to parent
		//var go = Instantiate(mp.prefab);
		//go.transform.SetParent(transform, false);
	}

	public void SetScale(float scale)
	{
		transform.localScale = new Vector3(scale, scale, scale);
	}

	public void OnComponentSelected(object par)
	{
		modelName = (string)par;
	}

	public void OnSpawnModel(object par)
	{
		// Destroy currentModel, if present.
		OnDestroyModel();

		var scenario = par as SimulatorScenario;
		string modelname = modelName + "-" + scenario.name;

		ModelPrefab mp = spawnableModels.FirstOrDefault(e => e.modelName == modelname);

		try
		{
			// Instanciate prefab and add to parent
			spawnedPrefab = Instantiate(mp.prefab);
			spawnedPrefab.transform.SetParent(transform, false);
		}
		catch(Exception ex)
		{
			throw new Exception(String.Format("Model [{0}] not found, ex: {1}", modelname, ex.Message));
		}
	}

	public void OnDestroyModel()
	{
		if(spawnedPrefab != null)
		{
			Destroy(spawnedPrefab);
			spawnedPrefab = null;
		}
	}
}
