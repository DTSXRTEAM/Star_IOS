using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.Base.Statistics
{

    public class StatisticsLogSecInView : StatisticsLogBase
    {
        [SerializeField]
        private float logUpdateRate;
        [SerializeField]
        private Transform targetCamera;

        private float secondsInView;
        private bool hasCountedThisFrame;
        private float timeAtLastUpdate;
        
        private void Update()
        {
            hasCountedThisFrame = false;

            if (secondsInView > 0f && Time.time - timeAtLastUpdate >= logUpdateRate)
            {
                timeAtLastUpdate = Time.time;
                statisticsLogger.Log(statisticsType, secondsInView);
                secondsInView = 0f;
            }
        }

        private void OnWillRenderObject()
        {
            RaycastHit hit;
            if (!hasCountedThisFrame && Physics.Raycast(targetCamera.position, transform.position - targetCamera.position, out hit))
            {
                if(hit.transform.gameObject == transform.gameObject)
                    secondsInView += Time.deltaTime;
            }
            hasCountedThisFrame = true;
        }
    }
}

