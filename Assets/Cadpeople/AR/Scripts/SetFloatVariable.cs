using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.AR
{
    public class SetFloatVariable : MonoBehaviour
    {

        [SerializeField]
        private FloatVariable floatVariable;

        public void SetValue(float value)
        {
            floatVariable.Value = value;
        }

    }
}


