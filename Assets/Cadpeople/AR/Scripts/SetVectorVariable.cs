using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.AR
{
    public class SetVectorVariable : MonoBehaviour {

        [SerializeField]
        private VectorThreeVariable vectorThreeVariable;

        private void Update()
        {
            vectorThreeVariable.Value = transform.position;
        }
    }
}


