using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Cadpeople.Base.I360Render
{
#if UNITY_EDITOR

    public class I360RenderEditor : EditorWindow {

        private enum ImageType { JPG, PNG }
        private string screenShotPath = "C:\\Cadpeople";
        private Camera camera;
        private ImageType imageType;
        private int width = 2048;
        [MenuItem("Cadpeople/Create 360 screenshot")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(I360RenderEditor), true, "360 screenshot");
        }

        void OnGUI()
        {
            GUILayout.Label("360 screenshot creater", EditorStyles.boldLabel);
            screenShotPath = EditorGUILayout.TextField("Screenshot path", screenShotPath);
            camera = (Camera)EditorGUILayout.ObjectField("Camera", camera, typeof(Camera), true);
            imageType = (ImageType)EditorGUILayout.EnumPopup("", imageType);
            width = EditorGUILayout.IntField("Width", width);

            if (GUILayout.Button("Take screenshot"))
            {
                var bytes = I360Render.Capture(width, (imageType == ImageType.JPG), camera);
                File.WriteAllBytes(screenShotPath + "\\360screenshot." + (imageType == ImageType.JPG ? "jpg" : "png"), bytes);
            }

        }

    }

#endif
}


