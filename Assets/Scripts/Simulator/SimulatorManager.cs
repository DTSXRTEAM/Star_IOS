using Cadpeople.Events;
using Cadpeople.Network.Socketio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

namespace SimulatorInterface
{
	public class SimulatorManager : Singleton<SimulatorManager>
	{
		// Events
		[Header("-- Simulator Calling events --")]
		public GameEvent logEvent = null;
		public GameEvent sendObjectEvent = null;
		public GameEvent scenarioLoadedEvent = null;
		public GameEvent componentReceivedEvent = null;
		public GameEvent currentComponentUpdatedEvent = null;


		public string currentScenario = "";
		public string ServerUrl { get; set; }
		public int ServerPort { get; set; }
		private ISocketHandler socketHandler = null;
		private List<string> componentListeners = new List<string>();

		public virtual void OnConnectionChanged(object val)
		{
			logEvent?.Raise("OnConnectionChanged Received");
			logEvent?.Raise("Initializing network manager");

			if (socketHandler != null)
			{
				Disconnect();
			}

			// get serverIp
			var cfg = CPSettings.Instance.GetConfig();

			ServerUrl = "http://" + cfg.serverIp + ":" + ServerPort;

			// Connect to simulator
			Connect("http://" + cfg.serverIp, ServerPort);
		}

		/// <summary>
		/// Called when there is some error from the socket
		/// </summary>
		/// <param name="data">Error data</param>
		protected virtual void OnError(string data)
		{
			Helper.FireEventInMainThread(logEvent, "Unknown error from server: " + data);
		}

		/// <summary>
		/// Connect to socket
		/// </summary>
		private void Connect(string address, int port)
		{
			logEvent?.Raise($"Connecting to {address}:{port}");

			// Get sockethandler from factory
			socketHandler = SocketHandlerFactory.Get(SocketHandlerTypes.SocketIoClientDotNet, address, port);

			// Add event listeners
			AddListener("loaded");
			AddListener("connected");
			AddListener("stopped");

			// Hock up to  general socket events 
			socketHandler.Error += OnError;
			socketHandler.DataReceived += OnDataReceived;

			// Connect
			socketHandler.Connect();
		}

		public void Disconnect()
		{
			logEvent?.Raise($"Disconnecting");
			ClearListeners();
			if (socketHandler != null)
			{
				socketHandler.ClearListeners();
				socketHandler.Disconnect();
				socketHandler = null;
			}
		}

		private void AddListener(string name)
		{
			// If we are not allready listening
			if (componentListeners.FirstOrDefault(e => e == name) == null)
			{
				// listen on socket
				socketHandler.AddListener(name);

				// Register to only listen once
				componentListeners.Add(name);
			}
		}

		private void ClearListeners()
		{
			componentListeners.Clear();
		}

		/// <summary>
		/// Start listen to all component events from the given list of components.
		/// </summary>
		/// <param name="components"></param>
		protected void ListenToComponentEvents(object[] components)
		{
			foreach (var jsonComponent in components)
			{
				var component = JsonConvert.DeserializeObject<SimulatorInterface.Component>(jsonComponent.ToString());
				AddListener(component.name);
				componentReceivedEvent?.Raise(jsonComponent.ToString());
			}
		}

		/// <summary>
		/// Load a scenario
		/// </summary>
		/// <param name="name">Name of the scenario</param>
		public virtual void LoadScenario(object name)
		{
			logEvent?.Raise("Loading scenario: " + name);
			var v = new { scenario = name };
			socketHandler.Send("load", v);
		}
		public virtual void ScenarioLoaded(object val)
		{
			SimulatorScenario scenario = (SimulatorScenario)val;
			logEvent?.Raise("Scenario loaded: " + scenario.name);
		}
		public virtual void UnloadScenario()
		{
			logEvent?.Raise("UnLoading scenario");
			var v = new { };
			socketHandler.Send("stop", v);
		}

		/// <summary>
		/// Send object to the server
		/// </summary>
		/// <param name="eventParams"></param>
		public virtual void SendObject(object par)
		{
			var param = ((SendObjectdata)par);
			Helper.FireEventInMainThread(logEvent, $"Send Object: {param.name}, Data {param.data}");
			socketHandler.Send(param.name, param.data);
		}

		protected virtual void OnDataReceived(string data, SocketPayload payload)
		{
			throw new System.Exception("Please override");
		}
	}
}
