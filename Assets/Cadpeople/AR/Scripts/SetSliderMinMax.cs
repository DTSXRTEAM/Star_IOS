using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cadpeople.AR
{
    public class SetSliderMinMax : MonoBehaviour {

        [SerializeField]
        private Cadpeople.AR.ARKit.CadpeopleARKitSettings arSettings;
        [SerializeField]
        private Slider slider;

        private void Awake()
        {
            slider.minValue = arSettings.modelMinSize;
            slider.maxValue = arSettings.modelMaxSize;
        }
    }
}


