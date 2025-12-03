using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Cadpeople.Storyline
{
    public class TransitionWhenCorrectTransform : ITransition {


        public GameObject objectToCheck;
        public Transform transformToCheckAgainst;
        [Space]
        public bool checkPosition;
        public bool checkRotation;

        [Space]
        [Tooltip("The tolerance distance")]
        public float positionTolerance;

        [Tooltip("The tolerance in degrees")]
        public float rotationTolerance;

        //-----------------------------------############---------------------------------\\





        //-----------------------------------############---------------------------------\\

        protected virtual void OnDrawGizmos()
        {
            if(objectToCheck && transformToCheckAgainst)
            {

                Gizmos.DrawWireSphere(transformToCheckAgainst.position, 0.1f);

                Transform t = transformToCheckAgainst;

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(t.position, t.position + t.forward * 0.1f);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(t.position, t.position + t.up * 0.1f);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(t.position, t.position + t.right * 0.1f);




                t = objectToCheck.transform;

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(t.position, t.position + t.forward * 0.1f);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(t.position, t.position + t.up * 0.1f);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(t.position, t.position + t.right * 0.1f);


                float sqrDist = (objectToCheck.transform.position - transformToCheckAgainst.position).sqrMagnitude;
                Gizmos.color = sqrDist < (positionTolerance * positionTolerance) ? Color.green : Color.red;
                Gizmos.DrawLine(objectToCheck.transform.position, transformToCheckAgainst.position);


                Gizmos.color = Color.white;

            }


        }


        public override bool ShouldTransition()
        {
            if (!objectToCheck || !transformToCheckAgainst) return false;

            if(checkPosition && checkRotation)
            {
                return CheckPosition() && CheckRotation() && base.ShouldTransition();
            }

            if (checkPosition && !checkRotation) return CheckPosition() && base.ShouldTransition();
            if (!checkPosition && checkRotation) return CheckRotation() && base.ShouldTransition();


            return false;
        }


        protected bool CheckPosition()
        {
            float sqrDist = (objectToCheck.transform.position - transformToCheckAgainst.position).sqrMagnitude;
            return sqrDist < (positionTolerance * positionTolerance);

        }


        protected bool CheckRotation()
        {

            float angle = Vector3.Angle(objectToCheck.transform.rotation.eulerAngles, transformToCheckAgainst.rotation.eulerAngles);

            return angle < rotationTolerance;


        }



    }

}

