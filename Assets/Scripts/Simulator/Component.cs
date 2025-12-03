using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorInterface
{
    public class Component
    {
        public string name;
        public string type;
        //public object custom;
    }

    public class ConnectedComponent : Component
    {
        public string id;
    }

    public class User : Component
    {
        public string position;
    }

    public class Toolbox : Component
    {
        public string[] decorators;
        public string tool;
        public string[] tools;
        public bool visible = false;
    }

	public class State : Component
	{
		public ValueData value;
	}

	public class Probe : Component
	{
		public PData data;
	}

	public class EmitterCond : ConnectedComponent
    {
        public Condition[] conditions;
        public StateData data;
    }
    public class Max : ConnectedComponent
    {
        public string dataType;
        public int length;
        public int value;
    }
	public class Floor : ConnectedComponent
	{
		public string dataType;
		public int value;
	}
	public class Bounds : ConnectedComponent
	{
		public string dataType;
		public float value;
	}
	public class Modifier : ConnectedComponent
	{
		public string dataType;
		public float value;
	}
	public class Switch : Component
    {
        public string dataType;
        public string id;
        public bool active;
    }
    public class Toggle : ConnectedComponent
    {
        public string dataType;
        public bool enabled;
        public bool momentary;
    }

	public class ToggleCond : ConnectedComponent
	{
		public string dataType;
		public bool enabled;
		public bool momentary;
	}

	public class Not : ConnectedComponent
	{
		public string dataType;
		public int value;
	}

	public class Throttle : ConnectedComponent
	{
		public float decrement;
		public float increment;
		public float interval;
		public float value;
	}
	public class Rotor : ConnectedComponent
	{
		public float rotation;
		public float rpm;
	}
	public class Table : ConnectedComponent
	{
		public float value;
	}

	public class Indicator : ConnectedComponent
	{
		public string dataType;
		public bool active;
	}
	public class Enumerator : ConnectedComponent
    {
        public string dataType;
        public int index;
        public int length;
        public bool loop;
    }

    public class Multimeter : Component
    {
        public string title;
        public bool visible;
        public int x;
        public int y;
        public int backlight;
        public string display;
        public bool fuse;
        public bool loop;
        public int mode;
        public ProbeData negative;
        public ProbeData positive;
        public bool power;
    }

	public class Manometer : Component
	{
		public string title;
		public bool visible;
		public int x;
		public int y;
		public string display;
		public ProbeData probe;
		public bool power;
	}

	public class Interface : Component
	{
		public int error;
	}

	public class ProbeData : Component
    {
        public string component;
        public string output;
    }

    public class Buffer : Component
    {
        public string[] history;
        public string value;
    }

    public class Condition : ConnectedComponent
    {
        public string dataType;
        public string value;
    }
	public class And : ConnectedComponent
	{
		public string dataType;
		public int value;
	}
	public class Or : ConnectedComponent
	{
		public string dataType;
		public int value;
	}

	public class Trigger : ConnectedComponent
	{
		public string dataType;
		public bool active;
	}

	public class Gate : ConnectedComponent
	{
		public bool enabled;
	}

	public class PData
	{
		public float value { get; set; }

		public object this[string propertyName]
		{
			get
			{
				try
				{
					return this.GetType().GetProperty(propertyName).GetValue(this, null);
				}
				catch (Exception)
				{

					throw new Exception("Missing property : " + propertyName);
				}
			}
			set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
		}
	}

	public class ValueData
	{
		public string siteName { get; set; }
		public string wtgId { get; set; }
		public string transformerType { get; set; }
		public string transformerSerial { get; set; }
		public string date { get; set; }
		public string oilTemp { get; set; }
		public string technicalName { get; set; }


		public object this[string propertyName]
		{
			get
			{
				try
				{
					return this.GetType().GetProperty(propertyName).GetValue(this, null);
				}
				catch (Exception)
				{

					throw new Exception("Missing property : " + propertyName);
				}
			}
			set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
		}
	}

	public class StateData
    {
		public int visible { get; set; }
		public int fluid { get; set; }
		public int hose { get; set; }
		public int cap { get; set; }
		public int cover { get; set; }
		public int filter { get; set; }
		public int bolts { get; set; }
		public int connected { get; set; }
		public int action { get; set; }
		public int open { get; set; }
		public int drain { get; set; }
		public int danger { get; set; }
		public int warning { get; set; }
		public int state { get; set; }


		public object this[string propertyName]
		{
			get
			{
				try
				{
					return this.GetType().GetProperty(propertyName).GetValue(this, null);
				}
				catch (Exception)
				{
 
					throw new Exception("Missing property : " + propertyName);
				}
			}
			set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
		}
    }


	public class Tool
	{
		public string id;
		public string name;
		public string description;
		public bool enabled;
	}

	public class ToolboxType
	{
		public string id;
		public string name;
		public Tool[] tools;
	}

}
