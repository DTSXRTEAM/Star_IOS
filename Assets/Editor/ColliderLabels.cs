using UnityEngine;
using System.Collections;
using UnityEditor;

// Create a 180 degrees wire arc with a ScaleValueHandle attached to the disc
// lets you visualize some info of the transform

[CustomEditor(typeof(HotspotParent))]
class ColliderLabels : Editor
{
	void OnSceneGUI()
	{
		HotspotParent hotspotParent = (HotspotParent)target;
		if (hotspotParent == null)
		{
			return;
		}

		Handles.color = Color.blue;

		foreach (Transform child in hotspotParent.transform)
		{
			Handles.Label(child.transform.position, child.transform.name);
		}
	}
}
