using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ColliderConnecter))]
public class ColliderConnecterEditor : Editor
{
	private ColliderConnecter myScript;
	private BoxCollider collider;
	private Transform transform;

	private void OnEnable()
	{
		myScript = (ColliderConnecter)target;
		collider = myScript.GetComponent<BoxCollider>();
		transform = myScript.GetComponent<Transform>();
	}
	public override void OnInspectorGUI()
	{

		GameObject go = EditorGUILayout.ObjectField("Assign gameobject here", null, typeof(GameObject), true) as GameObject;
		if (go != null)
		{
			Renderer renderer = go.transform.GetComponent<Renderer>();
			if (renderer != null)
			{
				// Set pos and bounds
				collider.size = renderer.bounds.size;
				transform.position = go.transform.position;
			}
		}
		DrawDefaultInspector();
	}

}


#endif
public class ColliderConnecter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
