using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace SimulatorInterface
{
    class ComponentFactory
    {
        /// <summary>
        /// Creates and return a component based on name and a given json representation of the component.
        /// </summary>
        /// <param name="baseComponent">The json deserialized as a base component</param>
        /// <param name="componentJson">The json representation of the component</param>
        /// <returns>The component</returns>
        public Component GetComponent(Component baseComponent, string componentJson)
        {
            switch (baseComponent.type)
            {
                case "Buffer":
                    return JsonConvert.DeserializeObject<Buffer>(componentJson);
                case "Condition":
                    return JsonConvert.DeserializeObject<Condition>(componentJson);
                case "CustomTool":
                    // Handle custom tools
                    return GetTool(baseComponent.name, componentJson);
                case "Max":
                    return JsonConvert.DeserializeObject<Max>(componentJson);
				case "Floor":
					return JsonConvert.DeserializeObject<Floor>(componentJson);
				case "Switch":
                    return JsonConvert.DeserializeObject<Switch>(componentJson);
                case "Toggle":
                    return JsonConvert.DeserializeObject<Toggle>(componentJson);
				case "ToggleCond":
					return JsonConvert.DeserializeObject<ToggleCond>(componentJson);
				case "Throttle":
					return JsonConvert.DeserializeObject<Throttle>(componentJson);
				case "Trigger":
					return JsonConvert.DeserializeObject<Trigger>(componentJson);
				case "Gate":
					return JsonConvert.DeserializeObject<Gate>(componentJson);
				case "Indicator":
					return JsonConvert.DeserializeObject<Indicator>(componentJson);
				case "Enumerator":
                    return JsonConvert.DeserializeObject<Enumerator>(componentJson);
                case "User":
                    return JsonConvert.DeserializeObject<User>(componentJson);
                case "Toolbox":
                    return JsonConvert.DeserializeObject<Toolbox>(componentJson);
                case "EmitterCond":
                    return JsonConvert.DeserializeObject<EmitterCond>(componentJson);
				case "Interface":
					return JsonConvert.DeserializeObject<Interface>(componentJson);
				case "And":
					return JsonConvert.DeserializeObject<And>(componentJson);
				case "Or":
					return JsonConvert.DeserializeObject<Or>(componentJson);
				case "Modifier":
					return JsonConvert.DeserializeObject<Modifier>(componentJson);
				case "State":
					return JsonConvert.DeserializeObject<State>(componentJson);
				case "Rotor":
					return JsonConvert.DeserializeObject<Rotor>(componentJson);
				case "Probe":
					return JsonConvert.DeserializeObject<Probe>(componentJson);
				case "Bounds":
					return JsonConvert.DeserializeObject<Bounds>(componentJson);
				case "Not":
					return JsonConvert.DeserializeObject<Not>(componentJson);
				case "Table":
					return JsonConvert.DeserializeObject<Table>(componentJson);
				default:
                    break;
            }

            return null;
        }
        /// <summary>
        /// Get tool based on name of the tool.
        /// </summary>
        /// <param name="toolName"></param>
        /// <param name="toolJson"></param>
        /// <returns>Tool as a component</returns>
        private Component GetTool(string toolName, string toolJson)
        {
            switch (toolName)
            {
                case "multimeter":
                    return JsonConvert.DeserializeObject<Multimeter>(toolJson);
				case "manometer":
					return JsonConvert.DeserializeObject<Manometer>(toolJson);
				default:
                    break;
            }

            return null;
        }
    }
}
