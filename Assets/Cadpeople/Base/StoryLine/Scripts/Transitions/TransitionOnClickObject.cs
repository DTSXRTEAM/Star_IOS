using Cadpeople.Storyline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.Storyline
{


    public class TransitionOnClickObject : ITransition
    {

        public GameObject objectToClick;


        protected bool shouldTransition = false;



        public override bool ShouldTransition()
        {
            
            if (shouldTransition)
            {
                shouldTransition = false;
                return true && base.ShouldTransition();
            }
            else
                return false;
        }

        // Use this for initialization
        void Start()
        {
            if (!parentState) parentState = GetComponentInParent<State>();
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo;
                Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(screenRay, out hitInfo))
                {
                    Collider c = objectToClick.GetComponent<Collider>();
                    if (!c) c = objectToClick.AddComponent<SphereCollider>();
                    if (hitInfo.collider == c)
                    {
                        shouldTransition = true;
                    }
                }

            }

        }


    }

}
