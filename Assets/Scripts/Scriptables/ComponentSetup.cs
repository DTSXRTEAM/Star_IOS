using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ComponentAR/ComponentSetup", order = 4)]
public class ComponentSetup : ScriptableObject
{
	public string imageName;
	public int port;
	public string componentName;
	public float scale;
}
