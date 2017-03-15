using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class Messenger : EditorWindow
{
    private string message = "";
    private SendMessageOptions Options = SendMessageOptions.RequireReceiver;
    private Transform[] selection = new Transform[0];

    [MenuItem("Window/Eric's Options/Messenger")]
    static void Init()
    {
        EditorWindow.GetWindow<Messenger>();
    }

    public void OnGUI()
    {
        GUI.enabled = (Application.isPlaying);

        selection = Selection.GetTransforms(SelectionMode.ExcludePrefab);

        EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Message", GUILayout.Width(60));
            message = EditorGUILayout.TextField(message);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Options", GUILayout.Width(60));
            Options = (SendMessageOptions) EditorGUILayout.EnumPopup(Options, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        GUI.enabled = (Application.isPlaying && selection.Length > 0 && message.Length > 0);
        bool send = GUILayout.Button("Send", GUILayout.Width(50));

        if (send)
        {
            foreach (var t in selection)
            {
                Debug.Log("Sending message '" + message + "' to " + t.gameObject.name);
            }

            Selection.GetTransforms(SelectionMode.Unfiltered).Foreach(t => t.SendMessage(message, Options));

            Repaint();
        }
    }

    public void OnInspectorUpdate()
    {
        Repaint();
    }
    
}