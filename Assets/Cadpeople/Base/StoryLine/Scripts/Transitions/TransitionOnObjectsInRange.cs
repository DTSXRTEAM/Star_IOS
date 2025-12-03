using Cadpeople.Storyline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cadpeople.Storyline
{


    public class TransitionOnObjectsInRange : ITransition
    {

        public GameObject object1;
        public GameObject object2;
        public float transitionWhenDistanceIsBelowValue = 0.5f;


        public override bool ShouldTransition()
        {
            

            if (!object1 || !object2) return false;
            float sqrDist = (object1.transform.position - object2.transform.position).sqrMagnitude;


            return sqrDist < (transitionWhenDistanceIsBelowValue * transitionWhenDistanceIsBelowValue) && base.ShouldTransition();


        }


        protected virtual void OnDrawGizmosSelected()
        {

            if(object1)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(object1.transform.position, Vector3.one * 0.05f);
            }
            if(object2)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(object2.transform.position, Vector3.one * 0.05f);
            }
            if(object1 && object2)
            {
                Gizmos.color = new Color(1f, 1f, 1f, 0.4f);
                Gizmos.DrawWireSphere(object2.transform.position, transitionWhenDistanceIsBelowValue);

                float sqrDist = (object1.transform.position - object2.transform.position).sqrMagnitude;
                Gizmos.color = sqrDist < (transitionWhenDistanceIsBelowValue * transitionWhenDistanceIsBelowValue) ? Color.green : Color.red;

                Gizmos.DrawLine(object1.transform.position, object2.transform.position);
            }

        }

    }

}