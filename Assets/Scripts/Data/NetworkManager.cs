using UnityEngine;
using System.Collections;
using System.Net;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Cadpeople.Events;
using Cadpeople.Network.Socketio;
using System.Linq;
using SimulatorInterface;

namespace Network
{
	class NetworkManager : SimulatorManager
    {
		// Events
		[Header("-- Network Calling events --")]
		public GameEvent showProcedureListEvent = null;
		public GameEvent screenFadeEvent = null;
		public GameEvent spawnScenarioModelEvent = null;
		public GameEvent modelChangedEventEvent = null;
		public GameEvent showStandardDialogEvent = null;
		public GameEvent showComponentsList = null;
		public GameEvent activityInfoChangedEvent = null;
		public GameEvent groupChangedEvent = null;

		[SerializeField] private MainComponentGroup MainComponentGroup;

		private string currentModelName = "";
		private string currentGroupName = "";

		public override void OnConnectionChanged(object val)
		{
			var setup = (ComponentSetup)val;
			currentModelName = setup.componentName;
			currentGroupName = setup.imageName;
			ServerPort = setup.port;

			base.OnConnectionChanged(val);

			groupChangedEvent?.Raise(ServerPort - 8030);
		}

		public void OnComponentPicked(object param)
		{
			currentModelName = (string)param;
		}

        /// <summary>
        /// Called when data is recieved from the socket.
        /// </summary>
        /// <param name="data">Data recieved from the socket</param>
        protected override void OnDataReceived(string data, SocketPayload payload)
        {
			// handle data
			switch (data)
            {
                case "connected":
					Helper.RunInMainThread(() =>
					{
						logEvent?.Raise("Connected received");

						var simulatorConnection = JsonConvert.DeserializeObject<SimulatorConnection>(payload.ObjectPayload.ToString());
						logEvent?.Raise("Connected to simulator Version: " + simulatorConnection.version);

						if (simulatorConnection.scenario != null && !String.IsNullOrWhiteSpace(simulatorConnection.scenario.name))
						{
							// Check if current model matches server model / scenario
							//CheckForModelMatch(simulatorConnection.scenario, true);

							currentModelName = GetComponentNameFromScenario(simulatorConnection.scenario.name);
							currentComponentUpdatedEvent.Raise(currentModelName);

							// Set current scenario 
							currentScenario = simulatorConnection.scenario.name;

							// set info
							var info = GetScenarioInfo(currentScenario);
							activityInfoChangedEvent?.Raise(info);

							// Spawn model
							spawnScenarioModelEvent?.Raise(simulatorConnection.scenario);

							// Scenario is already loaded
							ListenToComponentEvents(simulatorConnection.scenario.components);

							// Trigger events
							scenarioLoadedEvent?.Raise(simulatorConnection.scenario);
						}
						else
						{
							// Trigger event for showing procedurelist
							//showProcedureListEvent?.Raise(currentModelName);
							
							showComponentsList?.Raise(currentGroupName);
							screenFadeEvent?.Raise(false);
						}
						
					});
					break;
                case "loaded":
					Helper.RunInMainThread(() =>
					{
						// Convert to scenario object
						var scenario = JsonConvert.DeserializeObject<SimulatorScenario>(payload.ObjectPayload.ToString());
						logEvent?.Raise("Loaded scenario: " + scenario.name);

						// Check if current model matches server model / scenario
						//CheckForModelMatch(scenario);
						currentModelName = GetComponentNameFromScenario(scenario.name);
						currentComponentUpdatedEvent.Raise(currentModelName);

						// Set current scenario
						currentScenario = scenario.name;

						// Spawn model
						spawnScenarioModelEvent?.Raise(scenario);

						// Trigger load event
						ListenToComponentEvents(scenario.components);

						// Trigger event
						scenarioLoadedEvent.Raise(scenario);
					});
					break;
				case "stopped":
					Helper.RunInMainThread(() =>
					{
						// Trigger event for showing procedurelist
						showComponentsList?.Raise(null);
						screenFadeEvent?.Raise(false);
					});
					break;
				default:
					Helper.RunInMainThread(() =>
					{
						componentReceivedEvent?.Raise(payload.ObjectPayload);
					});                 
					break;
            }
        }

		/// <summary>
		/// Called when there is some error from the socket
		/// </summary>
		/// <param name="data">Error data</param>
		protected override void OnError(string data)
		{
			Helper.FireEventInMainThread(logEvent, "Unknown error from server: " + data);
		}

		private string GetComponentNameFromScenario(string scenario)
		{
			string componentName = "";
			foreach (var mc in MainComponentGroup.Components)
			{
				if (mc.Procedures.FirstOrDefault(e => e.SimulatorScenario == scenario) != null)
				{
					componentName = mc.QRModelKey;
					break;
				}
			}
			return componentName;
		}

		private string GetScenarioInfo(string scenario)
		{
			foreach (var mc in MainComponentGroup.Components)
			{
				var proc = mc.Procedures.FirstOrDefault(e => e.SimulatorScenario == scenario);
				if (proc != null)
				{
					return proc.Description;
				}
			}
			return "Not found";
		}

		private void CheckForModelMatch(SimulatorScenario scenario, bool showDialog = false)
		{
			// Does model match current running scenario ?
			var currentMC = MainComponentGroup.Components.FirstOrDefault(e => e.QRModelKey == currentModelName);
			var proc = currentMC.Procedures.FirstOrDefault(e => e.SimulatorScenario == scenario.name);
			if (proc == null)
			{
				// Does not match, give info
				if (showDialog)
				{
					showStandardDialogEvent?.Raise(new DialogSpawnInfo(10000));
				}

				// Find procedure where current scenario is
				string componentName = GetComponentNameFromScenario(scenario.name);

				var setup = new ComponentSetup
				{
					componentName = componentName,
					scale = -1 // No change in scale
				};
				modelChangedEventEvent?.Raise(setup);
				currentModelName = componentName;
			}
		}

		public override void ScenarioLoaded(object val)
		{
			base.ScenarioLoaded(val);
			screenFadeEvent?.Raise(false);
		}

		public override void LoadScenario(object name)
		{
			screenFadeEvent?.Raise(true);
			var info = GetScenarioInfo(name as string);
			activityInfoChangedEvent?.Raise(info);

			base.LoadScenario(name);
		}
    }
}
