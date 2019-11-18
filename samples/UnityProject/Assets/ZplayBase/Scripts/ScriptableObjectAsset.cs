#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using System.IO;


public static class ScriptableObjectAsset
{
    /// <summary>
    //  This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static T CreateAsset<T>(String assetName = null, Boolean focusOnProjectWindow = true) where T : ScriptableObject
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (path == "")
            path = "Assets/Resources";
        else if (Path.GetExtension(path) != "")
            path = path.Replace(Path.GetFileName(path), "");

        path += Path.DirectorySeparatorChar;

        if (String.IsNullOrEmpty(assetName))
        {
            assetName = typeof(T).Name;
        }
        T asset = ScriptableObject.CreateInstance<T>();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + assetName + ".asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        if (focusOnProjectWindow)
        {
            EditorUtility.FocusProjectWindow();
        }
        Selection.activeObject = asset;
        return asset;
    }
}

#endif