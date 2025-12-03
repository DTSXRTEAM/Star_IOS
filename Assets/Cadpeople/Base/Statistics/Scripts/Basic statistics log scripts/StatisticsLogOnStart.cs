using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.Base.Statistics
{

    public class StatisticsLogOnStart : StatisticsLogBase
    {
        [SerializeField]
        private int logCountPrStart;

        private void Start()
        {
            statisticsLogger.Log(statisticsType, logCountPrStart);
        }

    }

}


