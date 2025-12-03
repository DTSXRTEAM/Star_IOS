using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.Base.Statistics
{

    public abstract class StatisticsLogBase : MonoBehaviour
    {
        [SerializeField]
        protected StatisticsLogger statisticsLogger;
        [SerializeField]
        protected StatisticsType statisticsType;

    }

}


