using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorInterface
{
    class Connection
    {
        public SimulatorConnection connection;

		public Connection()
		{
			connection = null;
		}
    }

    class SimulatorConnection
    {
        public string version;
        public SimulatorScenario scenario;

		public SimulatorConnection()
		{
			version = "";
			scenario = null;
		}
    }

    class SimulatorScenario
    {
        public string name;
        public string title;
        public string description;
        public DateTime startTime;
        public string session;
        public object[] components;
        public bool enabled;

		public SimulatorScenario()
		{
			name = "";
			title = "";
			description = "";
			startTime = DateTime.MinValue;
			session = "";
			components = null;
			enabled = false;
		}
    }
}
