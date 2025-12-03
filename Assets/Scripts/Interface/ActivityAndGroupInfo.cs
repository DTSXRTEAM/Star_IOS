using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivityAndGroupInfo : MonoBehaviour
{
	public TextMeshProUGUI Text;

	private int currentGroup = -1;

	public void OnGroupChanged(object group)
	{
		Debug.Log("Setting group to " + group as string);
		if(group != null)
			currentGroup = (int)group;
	}

	public void OnActivityInfoChanged(object activityInfo)
	{
		Debug.Log("Setting info to " + activityInfo as string);
		Text.text = activityInfo as string + " - Group" + currentGroup;
	}

	public void OnClearInfo()
	{
		Text.text ="";
	}
}
