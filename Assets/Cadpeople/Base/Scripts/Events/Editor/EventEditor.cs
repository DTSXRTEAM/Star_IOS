using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Cadpeople.Events
{
    [CustomEditor(typeof(GameEvent))]
    public class EventEditor : Editor
    {

		List<GameObject> referencingObjects;
		List<string> componentNames;

		public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            GameEvent e = target as GameEvent;
            if (GUILayout.Button("Raise"))
                e.Raise("");

			GUI.enabled = true;
			if (GUILayout.Button("Find references"))
			{
				var rootGOs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
				referencingObjects = new List<GameObject>();
				componentNames = new List<string>();

				for (int i = 0; i < rootGOs.Length; i++)
				{

					RecursiveFindAllReferences(rootGOs[i], referencingObjects);	

				}

			}

			
			if(referencingObjects != null)
			{
				int i = 0;
				var centeredStyle = GUI.skin.GetStyle("Label");
				centeredStyle.alignment = TextAnchor.UpperCenter;
				//var btnStyle = GUI.skin.GetStyle("Button");
				//btnStyle.fixedWidth = 150;
				GUILayout.BeginHorizontal();

				GUILayout.Label("GAMEOBJECT", centeredStyle);
				GUILayout.Label("COMPONENT", centeredStyle);
				GUILayout.Label("SELECT", centeredStyle);

				GUILayout.EndHorizontal();

				foreach (var refObject in referencingObjects)
				{
					GUILayout.BeginHorizontal();
					
					GUILayout.Label(refObject.name, centeredStyle);

					GUILayout.Label(componentNames[i], centeredStyle);
					i++;

					if(GUILayout.Button("Select object"))
					{
						Selection.activeGameObject = refObject;
					}

					GUILayout.EndHorizontal();
				}

				if (referencingObjects.Count == 0)
					GUILayout.Label("Could not find any references in current scene.");

			} else
			{
				GUILayout.Label("Rescan for references to list them here.");
			}

        }

		private void RecursiveFindAllReferences(GameObject go, List<GameObject> objectsReferencing)
		{
			var components = go.GetComponents<Component>();

			for (int j = 0; j < components.Length; j++)
			{

                var fields = components[j].GetType().GetFields();
                var privateFields = components[j].GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                var fieldsList = new List<FieldInfo>(fields);
                fieldsList.AddRange(privateFields);
				fieldsList = fieldsList.Distinct().ToList();

                fields = fieldsList.ToArray();

                var properties = components[j].GetType().GetProperties();

				for (int w = 0; w < fields.Length; w++)
				{
					if (fields[w].FieldType == typeof(GameEvent))
					{
						if ((fields[w].GetValue(components[j]) as GameEvent) == (target as GameEvent))
						{
							objectsReferencing.Add(go);
							componentNames.Add(components[j].GetType().Name);
						}
					} else if(fields[w].FieldType == typeof(List<GameEvent>))
                    {
                        var gameEventList = (fields[w].GetValue(components[j]) as List<GameEvent>);

                        foreach (var ge in gameEventList)
                        {
                            if(ge == (target as GameEvent))
                            {
                                objectsReferencing.Add(go);
                                componentNames.Add(components[j].GetType().Name);
                            }
                        }

                    }

				}
			}

			if(go.transform.childCount > 0)
			{
				for (int i = 0; i < go.transform.childCount; i++)
				{
					RecursiveFindAllReferences(go.transform.GetChild(i).gameObject, objectsReferencing);
				}
			}

		}

    }
}
#endif
