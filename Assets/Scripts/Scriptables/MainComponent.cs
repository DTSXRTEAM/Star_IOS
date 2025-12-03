using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ComponentAR/MainComponent", order = 2)]
public class MainComponent : ScriptableObject {

	public string QRModelKey;
	public string ComponentName;
	public string Description;
	public Procedure[] Procedures;
}
