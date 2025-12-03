using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cadpeople.Base.I360Render;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Cadpeople.Base.Statistics
{

#if UNITY_EDITOR
    [CustomEditor(typeof(StatisticsHeatmapViewpoint))]
    public class StatisticsHeatmapViewpointEditor : Editor
    {

        private StatisticsHeatmapViewpoint myScript;

        private void Awake()
        {
            myScript = (StatisticsHeatmapViewpoint)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if(GUILayout.Button("Take screenshot"))
            {
                myScript.TakeScreenshot();
            }

            EditorUtility.SetDirty(myScript);
        }
    }
#endif

    public class StatisticsHeatmapViewpoint : MonoBehaviour
    {

        public string backgroundPath = "C:\\Cadpeople";
        public string maskPath = "C:\\Cadpeople";
        public int width = 2048;

        [ExecuteInEditMode]
        public void TakeScreenshot()
        {
            // Create background image
            var bytes = I360Render.I360Render.Capture(width, false, GetComponent<Camera>());
            File.WriteAllBytes(backgroundPath + "\\background.png", bytes);

            Color[] colorArray = new Color[width * (int)(width * 0.5f)];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < (int)(width * 0.5f); j++)
                {
                    var deltaW = 360f / ((float)width);
                    var azimuth = i * deltaW + 0.5f * deltaW;
                    var deltaH = 180f / (width * 0.5f);
                    var altitude = 90f - (j * deltaH + 0.5f * deltaH);
                    azimuth *= 0.0174532925f;
                    altitude *= 0.0174532925f;
                    var z = Mathf.Sin(altitude);
                    var hyp = Mathf.Cos(altitude);
                    var y = hyp * Mathf.Cos(azimuth);
                    var x = hyp * Mathf.Sin(azimuth);

                    var rayDir = (Quaternion.Euler(90f, 180f, 0f) * new Vector3(x, y, z));

                    RaycastHit hit;
                    if(Physics.Raycast(transform.position, rayDir, out hit))
                    {
                        if(hit.transform.GetComponent<StatisticsLogHeatmapObject>() != null)
                        {
                            colorArray[i + j * width] = new Color(hit.transform.GetComponent<StatisticsLogHeatmapObject>().id / 255f, 0f, 0f, 1f);
                        }
                    } else
                    {
                        var indx = i  + j * width;
                        if (indx >= colorArray.Length)
                            Debug.Log(indx);
                        else
                            colorArray[indx] = new Color(0f, 0f, 0f, 1f);
                    }

                }
            }

            var texture = new Texture2D(width, (int)(width * 0.5f), TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;
            
            texture.SetPixels(0, 0, width, (int)(width * 0.5f),colorArray);
            texture.Apply();
            var maskBytes = ImageConversion.EncodeToPNG(texture);
            File.WriteAllBytes(maskPath + "\\mask.png", maskBytes);
        }
    }


}