using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class CPPackageImporter : EditorWindow {

    private string packagesRootPath = "A:\\External\\Unity\\Unity modules";
    private bool showPath;

    [MenuItem("Cadpeople/Package importer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CPPackageImporter), true, "Cadpeople package importer");
    }

    private void OnGUI()
    {

        GUILayout.Label("Cadpeople package importer", EditorStyles.boldLabel);
        GUILayout.Space(20);
        showPath = GUILayout.Toggle(showPath, "Show path to modules");

        if (showPath)
        {
            packagesRootPath = EditorGUILayout.TextField("Modules path", packagesRootPath);
        }

        GUILayout.Space(20);

        if (!Directory.Exists(packagesRootPath))
            return;

        var packages = Directory.GetFiles(packagesRootPath, "*.unitypackage", SearchOption.AllDirectories);
        var currentFolders = new List<string>();
        foreach (var package in packages)
        {

            var folders = package.Replace(packagesRootPath, "").Replace(Path.GetFileName(package), "").Split('\\').Where(s => s != "").ToList();

            for (int i = 0; i < folders.Count; i++)
            {
                if (i < currentFolders.Count && currentFolders[i] != folders[i])
                {
                    for (int j = currentFolders.Count - 1; j >= i; j--)
                    {
                        currentFolders.RemoveAt(j);
                        GUILayout.EndVertical();
                    }
                }
                if (i >= currentFolders.Count)
                {
                    GUILayout.BeginVertical("Box");
                    GUILayout.Label(folders[i]);
                    currentFolders.Add(folders[i]);
                    continue;
                }
            }
            if (GUILayout.Button("Import package"))
            {
                AssetDatabase.ImportPackage(package, false);
            }
        }

        foreach (var folder in currentFolders)
        {
            GUILayout.EndVertical();
        }
    }

}
#endif