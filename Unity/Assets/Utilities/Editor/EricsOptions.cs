using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Autosaver : EditorWindow
{
    private bool _enableAutosave;

    [MenuItem("Window/Eric's Options/Autosaver")]
    static void Init()
    {
        EditorApplication.playmodeStateChanged = CheckSave;

        Autosaver window = (Autosaver)EditorWindow.GetWindow<Autosaver>();
        window.maxSize = new Vector2(200, 100);
        window.minSize = new Vector2(10, 10);
        window._enableAutosave = EditorPrefs.GetBool("AutoSave");
    }

    public void OnGUI()
    {
        _enableAutosave = EditorGUILayout.Toggle("Save scene on Play", _enableAutosave);
        EditorPrefs.SetBool("AutoSave", _enableAutosave);
    }

    private static void CheckSave()
    {
        if (EditorPrefs.GetBool("AutoSave") && EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
        {
            Debug.Log("Auto-Saving scene before entering Play mode: " + EditorApplication.currentScene);

            EditorApplication.SaveScene();
            AssetDatabase.SaveAssets();
        }
    }
}