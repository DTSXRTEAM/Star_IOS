using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cadpeople.Storyline;


public class ActionSetActiveScript : IAction
{

    public MonoBehaviour scriptToSet;
    public bool setActive;

    public override void Execute()
    {
        if (scriptToSet) scriptToSet.enabled = setActive;
    }
}
