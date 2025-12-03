using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.AR
{
    public class SizeModifier : MonoBehaviour
    {

        [SerializeField]
        private FloatVariable sizeFactor;
        [SerializeField]
        private BoolVariable isDisabled;

        private Vector3 startSize;

        private void Awake()
        {
            startSize = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            if(!isDisabled.Value)
                transform.localScale = startSize * sizeFactor.Value;
        }
    }

}
