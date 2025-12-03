using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Cadpeople.AR
{
    public class HeightModifier : MonoBehaviour
    {
        [SerializeField]
        private FloatVariable heightFactor;
        [SerializeField]
        private Cadpeople.AR.ARKit.CadpeopleARKitSettings arSettings;

        private float startHeight;
        private float minY;
        private float maxY;

        private void Awake()
        {
            startHeight = transform.localPosition.y;
            minY = arSettings.modelMinHeight;
            maxY = arSettings.modelMaxHeight;
        }

        private void Update()
        {
            SetYAxis(heightFactor.Value);
        }

        public void SetYAxis(float v)
        {
            transform.localPosition = new Vector3(transform.localPosition.x,
                startHeight + (minY + (v * (maxY - minY))), transform.localPosition.z);
        }

        public void OnModelPlaced()
        {
            startHeight = transform.localPosition.y;
        }
    }
}
