using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Cadpeople.Base.Statistics
{
#if UNITY_EDITOR
    [CustomEditor(typeof(StatisticsLogHeatmapObject)), CanEditMultipleObjects]
    public class StatisticsLogObjectInViewEditor : Editor
    {

        private StatisticsLogHeatmapObject myScript;

        private void OnEnable()
        {
            myScript = (StatisticsLogHeatmapObject)target;
            
        }
        
        public override void OnInspectorGUI()
        {

            EditorGUILayout.LabelField("Current objects being logged (in current scene): " + FindObjectsOfType<StatisticsLogHeatmapObject>().Length.ToString());
            
            DrawDefaultInspector();

            if (myScript.id < 1 || myScript.id > 255)
                EditorGUILayout.LabelField("Invalid id");

            EditorUtility.SetDirty(myScript);

        }
    }
#endif

    public class StatisticsLogHeatmapObject : MonoBehaviour {

        [SerializeField]
        private StatisticsLogger statisticsLogger;
        [SerializeField]
        private Transform targetCamera;
        [SerializeField]
        private float updateRate;
        
        public int id = 1;

        private bool isInView;
        private bool inViewLastFrame;
        private float timeAtLastUpdate;

        private float timeAtStartLooking;

	    // Use this for initialization
	    void Start () {
            timeAtLastUpdate = -updateRate;
        }
	
	    // Update is called once per frame
	    void Update () {

            if(inViewLastFrame != isInView)
            {
                if (isInView)
                {
                    // Started looking at object
                    timeAtStartLooking = Time.time;
                } else
                {
                    // Not looking at object anymore
                    LogCurrentTime();
                }
            }

            inViewLastFrame = isInView;
            if (Time.time - timeAtLastUpdate >= updateRate)
            {
                isInView = false;
            }
        }

        private void OnDisable()
        {
            if(isInView)
                LogCurrentTime(timeAtLastUpdate);
        }

        private void LogCurrentTime(float overrideEndTime = -1f)
        {
            var newViewLog = new Base_data_models.StatisticsObjectViewLog
            {
                startTime = timeAtStartLooking,
                endTime = overrideEndTime == -1f ? Time.time : overrideEndTime,
                objectId = id
            };
            statisticsLogger.LogObjectInView(newViewLog);
        }

        private void OnWillRenderObject()
        {
            if(Time.time - timeAtLastUpdate >= updateRate)
            {
                
                timeAtLastUpdate = Time.time;

                RaycastHit hit;
                if (Physics.Raycast(targetCamera.position, transform.position - targetCamera.position, out hit))
                {
                    if (hit.transform.gameObject == gameObject)
                    {
                        isInView = true;
                    }
                }
            }
        }

    }
}


