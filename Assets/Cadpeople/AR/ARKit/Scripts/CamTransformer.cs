using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using TMPro;
namespace Cadpeople.AR.ARKit
{
    public class CamTransformer : MonoBehaviour
    {
        [SerializeField]
        private Camera normalCamera;
        [SerializeField]
        private Camera arCamera;
        [SerializeField]
        private VectorThreeVariable arOriginPosition;
        [SerializeField]
        private FloatVariable scaleFactor;
        [SerializeField]
        private BoolVariable isARScalingEnabled;

        private bool isTracking;

        void LateUpdate()
        {
            if(isARScalingEnabled.Value && isTracking)
            {
                
                Matrix4x4 matrix = UnityARSessionNativeInterface.GetARSessionNativeInterface().GetCameraPose();
                float invScale = 1.0f / scaleFactor.Value;
                Vector3 cameraPos = UnityARMatrixOps.GetPosition(matrix);
                Vector3 vecAnchorToCamera = cameraPos - arOriginPosition.Value;
                normalCamera.transform.localPosition = arOriginPosition.Value + (vecAnchorToCamera * invScale);
                normalCamera.transform.localRotation = UnityARMatrixOps.GetRotation(matrix);

                //this needs to be adjusted for near/far
                normalCamera.projectionMatrix = UnityARSessionNativeInterface.GetARSessionNativeInterface().GetCameraProjection();
            } else
            {
                Matrix4x4 matrix = UnityARSessionNativeInterface.GetARSessionNativeInterface().GetCameraPose();
                Vector3 cameraPos = UnityARMatrixOps.GetPosition(matrix);

                normalCamera.transform.localPosition = cameraPos;
                normalCamera.transform.localRotation = UnityARMatrixOps.GetRotation(matrix);

                //this needs to be adjusted for near/far
                normalCamera.projectionMatrix = UnityARSessionNativeInterface.GetARSessionNativeInterface().GetCameraProjection();
            }
        }

        public void StartTracking()
        {
            isTracking = true;
        }
    }
}
