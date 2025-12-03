using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
	public class CPCondition
	{
		public string model;
		public string key;
		public string value;
		public string type; 
	};

	public class CPAction
	{
		public string model;
		public string key;
		public string value;
	}

	public class CPTool
	{
		public string title;
		public List<CPToolAction> toolactions;
	}

	public class CPToolAction
	{
		public string title;
		public bool autorun;
		public List<CPCondition> conditions;
		public List<CPAction> actions;
	}


	public class CPInteraction
	{
		public string identifier;
		public List<CPTool> tools;
	}


	public class MenuAction
	{
		public string title;
		public bool autorun;
		public List<SendObjectdata> actionsToSend;
	}



}
