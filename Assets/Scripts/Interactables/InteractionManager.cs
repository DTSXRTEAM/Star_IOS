using Cadpeople.Events;
using Data;
using Newtonsoft.Json;
using SimulatorInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(ComponentManager))]
public class InteractionManager : CPEventListener {

	private List<CPInteraction> Interactions;
	private ComponentManager componentManager;

	[Header("-- Events --")]
	public GameEvent logEvent;
	public GameEvent showInteractionMenuEvent;
	public GameEvent sendObjectToServerEvent;
	public GameEvent highlightComponentEvent;

	private string modelName = "";

	public override void OnEventRaised(object par)
	{
		var setup = (ComponentSetup)par;
		modelName = setup.componentName;

		componentManager = GetComponent<ComponentManager>();
		logEvent?.Raise("Initializing interaction manager");
		Interactions = null;
	}

	public void OnInitializeInteractions(object par)
	{
		var scenario = par as SimulatorScenario;
		 
		StartCoroutine(LoadLocalConfig("Scenarios/" + scenario.name, OnConfigLoaded));
	}


	public IEnumerator LoadLocalConfig(string configFileName, Action<List<CPInteraction>> loadCallback)
	{
		string uri = "file://" + Application.streamingAssetsPath + "/" + configFileName + ".json";

		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();

			string[] pages = uri.Split('/');
			int page = pages.Length - 1;

			if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.Log(pages[page] + ": Error: " + webRequest.error);
			}
			else
			{
				List<CPInteraction> entries = JsonConvert.DeserializeObject<List<CPInteraction>>(webRequest.downloadHandler.text);
				loadCallback(entries);
			}
		}
	}

	public void OnClickedOnObject(object name)
	{
		// Is point/compoment in list
		var interactionPoint = Interactions.FirstOrDefault(e => e.identifier == (string)name);
		List<MenuAction> menuList = new List<MenuAction>();
		if (interactionPoint != null)
		{

			// Get Current tool
			Toolbox toolbox = (SimulatorInterface.Toolbox)componentManager.GetStoredComponent("toolbox");
			var tool = interactionPoint.tools.FirstOrDefault(e => e.title == toolbox.tool);
			if (tool != null)
			{
				tool.toolactions.ForEach(ta =>
				{
					if (IsValidToolAction(ta)) //ta.actions;	
					{
						var actions = ta.actions;
						if (actions != null && actions.Count > 0)
						{

							var ma = new MenuAction
							{
								title = ta.title,
								autorun = ta.autorun,
								actionsToSend = new List<SendObjectdata>()
							};

							actions.ForEach(a =>
							{
								CPAction action = a;
								object dataObject = null;
								switch (a.key)
								{
									case "value":
										dataObject = new { value = int.Parse(a.value) };
										break;
									case "active":
										dataObject = new { active = bool.Parse(a.value) };
										break;
									case "enabled":
										dataObject = new { enabled = bool.Parse(a.value) };
										break;
									case "index":
										dataObject = new { index = int.Parse(a.value) };
										break;
									case "tool":
										dataObject = new { tool = a.value};
										break;
									default:
										logEvent?.Raise("Unsuported key in InteractionManager, key : " + a.key);
										break;
								}
								ma.actionsToSend.Add(new SendObjectdata { name = a.model, data = dataObject });
							});
							menuList.Add(ma);
						}
					}
				});
			}
		}

		if (menuList.Count == 1 && menuList[0].autorun == true)
		{
			menuList[0].actionsToSend.ForEach(ac =>
			{
				sendObjectToServerEvent?.Raise(ac);
			});
			highlightComponentEvent?.Raise("");
		}
		else if(menuList.Count > 0)
		{
			highlightComponentEvent?.Raise(name as string);
			// Send event with menu points
			showInteractionMenuEvent?.Raise(menuList);
		}
	}

	public void OnClickedOnTool(object par)
	{
		var toolname = (string)par;

		Toolbox toolbox = (SimulatorInterface.Toolbox)componentManager.GetStoredComponent("toolbox");

		var targetTool = "pointer";
		if (toolbox.tool != toolname)
		{
			targetTool = toolname;
		}

		// Go back to pointer
		var d = new SendObjectdata
		{
			name = "toolbox",
			data = new { tool = targetTool }
		};
		sendObjectToServerEvent?.Raise(d);
	}

	private bool IsValidToolAction(CPToolAction ta)
	{
		bool isValid = true;
		ta.conditions.ForEach(cond =>
		{
			// Get model
			var model = componentManager.GetStoredComponent(cond.model);
			if(model == null)
			{
				throw new Exception("Error getting component:" + cond.model);
			}

			if (model.GetType() == typeof(SimulatorInterface.Toggle))
			{
				var toggle = model as SimulatorInterface.Toggle;
				if (toggle.enabled != bool.Parse(cond.value))
				{
					isValid = false;
				}
			}
			else if (model.GetType() == typeof(SimulatorInterface.ToggleCond))
			{
				var toggle = model as SimulatorInterface.ToggleCond;
				if (toggle.enabled != bool.Parse(cond.value))
				{
					isValid = false;
				}
			}
			else if (model.GetType() == typeof(SimulatorInterface.Indicator))
			{
				var indicator = model as SimulatorInterface.Indicator;
				if (indicator.active != bool.Parse(cond.value))
				{
					isValid = false;
				}
			}
			else if (model.GetType() == typeof(SimulatorInterface.Enumerator))
			{
				var enumertor = model as SimulatorInterface.Enumerator;
				if (string.IsNullOrWhiteSpace(cond.type))
				{
					if (enumertor.index != int.Parse(cond.value))
					{
						isValid = false;
					}
				}
				else if(cond.type == "greater" && enumertor.index <= int.Parse(cond.value))
				{
					isValid = false;
				}
				else if (cond.type == "lesser" && enumertor.index >= int.Parse(cond.value))
				{
					isValid = false;
				}
			}
			else if (model.GetType() == typeof(SimulatorInterface.Trigger))
			{
				var trigger = model as SimulatorInterface.Trigger;
				if (trigger.active != bool.Parse(cond.value))
				{
					isValid = false;
				}
			}
			else if (model.GetType() == typeof(SimulatorInterface.Gate))
			{
				var gate = model as SimulatorInterface.Gate;
				if (gate.enabled != bool.Parse(cond.value))
				{
					isValid = false;
				}
			}
			else if (model.GetType() == typeof(SimulatorInterface.EmitterCond))
			{
				var emitterCond = model as SimulatorInterface.EmitterCond;

				var val = emitterCond.data[cond.key];

				if (((int)emitterCond.data[cond.key]) != int.Parse(cond.value))
				{
					isValid = false;
				}
			}
			else if (model.GetType() == typeof(SimulatorInterface.And))
			{
				var and = model as SimulatorInterface.And;
				if (and.value != int.Parse(cond.value))
				{
					isValid = false;
				}
			}
			else
			{
				logEvent?.Raise("Unhandled type in IsValidToolAction, type: " + model.GetType());
			}
		});

		return isValid;
	}

	private void OnConfigLoaded(List<CPInteraction> localConfig)
	{
		Interactions = localConfig;
	}


}
