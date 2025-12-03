using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cadpeople.Storyline;


public class ActionResetScript : IAction {

    public List<MonoBehaviour> scriptsToReset = new List<MonoBehaviour>();



    public override void Execute()
    {
        foreach (MonoBehaviour m in scriptsToReset) m.SendMessage("Reset");
    }


}
