using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public static class AssetUtilities
{
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string typeNameSpaced = Regex.Replace(typeof(T).ToString(), @"(\p{Lu})", " $1").TrimStart();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeNameSpaced + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

    }

    //[MenuItem("Assets/Create/Basic Upgrade")]
    //public static void CreateAsset_Upgrade()
    //{
    //    AssetUtilities.CreateAsset<BasicUpgrade>();
    //}

}
