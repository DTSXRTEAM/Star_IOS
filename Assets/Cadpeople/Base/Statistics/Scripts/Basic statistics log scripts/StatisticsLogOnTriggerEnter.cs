using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.Base.Statistics
{
    public class StatisticsLogOnTriggerEnter : StatisticsLogBase
    {
        [SerializeField]
        private LayerMask layerMask;

        private void OnTriggerEnter(Collider other)
        {
            if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            {
                statisticsLogger.Log(statisticsType, 1);

            }
        }

    }
}


