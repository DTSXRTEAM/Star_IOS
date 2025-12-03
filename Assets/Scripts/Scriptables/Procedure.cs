using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ComponentAR/Procedure", order = 3)]
public class Procedure : ScriptableObject {

	public string Header;
	public string Description;
	public string SimulatorScenario;
	public bool IsActive;
}
