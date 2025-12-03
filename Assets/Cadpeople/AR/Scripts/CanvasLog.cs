using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Cadpeople.AR
{
    public class CanvasLog : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI logTextMesh;

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }
        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
			if(logString.StartsWith("Screen position"))
			{
				return;
			}

            logTextMesh.text += logString + Environment.NewLine;
//            logTextMesh.text += stackTrace + Environment.NewLine;
        }
    }
}

