using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Cadpeople.AR
{
    public class RotationModifier : MonoBehaviour
    {

        [SerializeField]
        private FloatVariable rotationFactor;

        private void Update()
        {
            SetRotation(rotationFactor.Value);
        }

        public void SetRotation(float v)
        {
            transform.rotation = Quaternion.AngleAxis(v, Vector3.up);
        }
    }
}
