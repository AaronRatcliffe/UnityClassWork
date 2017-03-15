using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Reflection;


[CustomPropertyDrawer(typeof(MultiEnumFlagsAttribute))]
public class MultiEnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
    }
}

[CustomPropertyDrawer(typeof(SingleEnumFlagAttribute))]
public class SingleEnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int bit = (int)Math.Log(property.intValue, 2);
        int newBit = EditorGUI.Popup(position, label.text, bit, property.enumNames);

        property.intValue = (1 << newBit);
    }
}


[CustomPropertyDrawer(typeof(LongStringAttribute))]
public class LongStringAttributeDrawer : PropertyDrawer
{
    private const int TAB = 15;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float LabelHeight = EditorStyles.label.CalcHeight(new GUIContent(label.text), Screen.width - 20);
        position.height = LabelHeight;
        EditorGUI.LabelField(position, label);

        EditorStyles.textField.wordWrap = true;
        position.height = EditorStyles.textField.CalcHeight(new GUIContent(property.stringValue), Screen.width - 20 - TAB);
        position.y += LabelHeight;
        position.x += TAB;
        position.width -= TAB;

        property.stringValue = EditorGUI.TextField(position, property.stringValue);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorStyles.textField.CalcHeight(new GUIContent(property.stringValue), Screen.width - 20 - TAB) +
               EditorStyles.label.CalcHeight(new GUIContent(label.text), Screen.width - 20);
    }
}

[CustomPropertyDrawer(typeof(ValidRegexAttribute))]
public class ValidRegexDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float height = EditorStyles.label.CalcHeight(label, int.MaxValue);
        position.height = height;

        ValidRegexAttribute regexAttribute = attribute as ValidRegexAttribute;
        string regex = regexAttribute.Regex;

        string newValue = EditorGUI.TextField(position, label, property.stringValue);

        if (!Regex.IsMatch(newValue, regex))
        {
            Rect errorPosition = position;
            errorPosition.y += height;

            GUIStyle redStyle = new GUIStyle(EditorStyles.label);
            redStyle.normal.textColor = Color.red;
            redStyle.fontStyle = FontStyle.Bold;

            EditorGUI.LabelField(errorPosition, " ", "Invalid Format!", redStyle);
        }

        property.stringValue = newValue;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ValidRegexAttribute regexAttribute = attribute as ValidRegexAttribute;
        string regex = regexAttribute.Regex;

        if (Regex.IsMatch(property.stringValue, regex))
        {
            return EditorStyles.label.CalcHeight(label, int.MaxValue);
        }
        else
        {
            return EditorStyles.label.CalcHeight(label, int.MaxValue) * 2;
        }
    }

    [CustomEditor(typeof(MessageTranslator))]
    public class LevelScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MessageTranslator targetTranslator = (MessageTranslator)target;

            for (int i = 0; i < targetTranslator.translations.Count; i++ )
            {
                Translation t = targetTranslator.translations[i];

                EditorGUILayout.BeginHorizontal();
                    t.From = EditorGUILayout.TextField(t.From);
                    EditorGUILayout.LabelField("=>", GUILayout.Width(20));
                    t.To = EditorGUILayout.TextField(t.To);
                    bool remove = GUILayout.Button("-", GUILayout.Width(25));
                EditorGUILayout.EndHorizontal();

                if (remove)
                {
                    targetTranslator.translations.RemoveAt(i);
                    i--;
                }
            }

            EditorGUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.ExpandWidth(true));
                bool add = GUILayout.Button("+", GUILayout.Width(25));
                GUILayout.Label("", GUILayout.ExpandWidth(true));
                GUILayout.Label("", GUILayout.Width(25));
            EditorGUILayout.EndHorizontal();

            if (add)
            {
                targetTranslator.translations.Add(new Translation());
            }
        }
    }
}
