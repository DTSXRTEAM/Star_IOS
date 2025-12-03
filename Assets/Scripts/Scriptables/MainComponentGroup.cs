using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ComponentAR/MainComponentGroup", order = 1)]
public class MainComponentGroup : ScriptableObject
{
	public List<MainComponent> Components;
}
