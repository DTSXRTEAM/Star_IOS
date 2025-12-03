using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Cadpeople.Events;
using System.Linq;

namespace SimulatorInterface
{
    class ComponentManager : Singleton<ComponentManager>
    {
        private Dictionary<string, Component> componentDict;
        private ComponentFactory factory;

		private ToolboxType toolboxType;

		[Header("-- Events --")]
		public GameEvent logEvent;
		public GameEvent componentUpdatedEvent;

		// Properties
		public ToolboxType GetToolbox()
		{
			return toolboxType;
		}

		public void SetCompleteToolbox(string text)
		{
			toolboxType = JsonConvert.DeserializeObject<ToolboxType[]>(text)[0]; 
		}

		void Start()
        {
			logEvent?.Raise("Initializing component manager");
            componentDict = new Dictionary<string, Component>();
            factory = new ComponentFactory();
		}
		
        /// <summary>
        /// Get stored component.
        /// </summary>
        /// <param name="name">Name of the component</param>
        /// <returns>The requested component. Null if there is no stored component with the given name.</returns>
        public Component GetStoredComponent(string name)
        {
            if (componentDict.ContainsKey(name))
            {
                return componentDict[name];
            }

			logEvent?.Raise($"Trying to get a component ({name}) that has not been loaded yet.");
            return null;
        }

        /// <summary>
        /// Called when component is updated from the server
        /// </summary>
        /// <param name="eventParams"></param>
        public void OnComponentReceived(object par)
        {
            // Get component as json and convert it to base component
            var json = par.ToString();
            var baseComponent = JsonConvert.DeserializeObject<Component>(json);
            // Get specific component based on component type
            var component = factory.GetComponent(baseComponent, json);
            if (component == null)
				// todo: log out untracked components
                return;
            // Add or update component
            if (!componentDict.ContainsKey(component.name))
            {
                componentDict.Add(component.name, component);
            } else
            {
                componentDict[component.name] = component;
            }
			// Trigger update component event (component as param 0)
			componentUpdatedEvent?.Raise(component);
        }
	}
}
