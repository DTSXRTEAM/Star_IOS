using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cadpeople.AR.ARKit
{

#if UNITY_EDITOR

    using UnityEditor;

    [CustomEditor(typeof(CadpeopleARKitSettings))]
    public class CadpeopleARKITSettingsEditor : Editor
    {

 //       CadpeopleARKitSettings myScript;

        private void Awake()
        {
 //           myScript = (CadpeopleARKitSettings)target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Cadpeople AR Kit module settings.", EditorStyles.boldLabel);
            GUILayout.TextArea("Change global settings for the AR module in this file.");

            GUILayout.Space(20);
            
            DrawDefaultInspector();

        }
    }

#endif
    
    [CreateAssetMenu(menuName = "cadpeople.ar.arkit/Settings")]
    public class CadpeopleARKitSettings : ScriptableObject
    {
        [SerializeField]
        private TrackingType[] trackingTypes;
        
        public TrackingType[] TrackingTypes { get { return trackingTypes; } }

        public ARReferenceImagesSet arImageReferenceSet;

        public bool useARCameraScaling;

        public float modelMovementStepSize;

        public float modelMaxHeight;

        public float modelMinHeight;

        public float modelMinSize;

        public float modelMaxSize;

    }
}


