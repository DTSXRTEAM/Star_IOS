using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cadpeople.Storyline
{


    public class TransitionWhenTriggered : ITransition
    {
        bool trigger = false;

        public override bool ShouldTransition()
        {
            if(trigger)
            {
                trigger = false;
                return true;
            }
            return false;
        }

        public void Trigger()
        {
            trigger = true;
        }

    }


}