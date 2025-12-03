using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.Base.Statistics
{

    public class StatisticsLogOnEvent : StatisticsLogBase
    {
        

        public void OnEventTriggered(object param)
        {
            statisticsLogger.Log(statisticsType, 1);
        }

    }
}
