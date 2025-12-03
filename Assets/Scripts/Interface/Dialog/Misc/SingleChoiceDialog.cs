using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class SingleChoiceDialog : MonoBehaviour
{
	public string stateComponentName;

	ToggleGroup ToggleGroupScript = null;

	private void Awake()
	{
		ToggleGroupScript = GetComponent<ToggleGroup>();
	}

	private void Start()
	{
		ToggleGroupScript.SetAllTogglesOff();

		// Get current state
		var breakerEnumerator = SimulatorInterface.ComponentManager.Instance.GetStoredComponent(stateComponentName);
		if (breakerEnumerator != null)
		{
			var index = ((SimulatorInterface.Enumerator)breakerEnumerator).index;
			if (index > 0)
			{
				var list = transform.GetComponentsInChildren<Toggle>();
				list[index - 1].isOn = true;
			}
			else
			{
				ToggleGroupScript.SetAllTogglesOff();
			}
		}
	}

	public void OnClose()
	{
        Toggle theActiveToggle = ToggleGroupScript.ActiveToggles().FirstOrDefault();
		if(theActiveToggle != null)
        {
			var toggleIndex = theActiveToggle.GetComponent<ToggleData>().index;

			var bdScript = GetComponent<BaseDialogTool>();

			// Send result
			var evt = new SendObjectdata
			{
				name = stateComponentName,
				data = new { index = toggleIndex }

			};
			bdScript.sendOjectToServerEvent?.Raise(evt);

			GetComponent<BaseDialogTool>().OnClose();
        }
    }
}
