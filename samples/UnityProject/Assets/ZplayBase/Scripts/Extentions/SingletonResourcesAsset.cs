using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Assets.Scripts.Extentions
{
    public abstract class SingletonResourcesAsset<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(AssetName);

                    if (_instance == null)
                    {
#if UNITY_EDITOR
                        var path = EditorExtentions.FindAssetPath(AssetName + ".asset");
                        if (path != null)
                        {
                            _instance = Resources.Load<T>(path);
                            if (_instance != null)
                            {
                                if (_instance is SingletonResourcesAsset<T>)
                                    (_instance as SingletonResourcesAsset<T>).OnInstantiation();
                                return _instance;
                            }
                        }
                        _instance = ScriptableObjectAsset.CreateAsset<T>();
                        if (_instance is SingletonResourcesAsset<T>)
                            (_instance as SingletonResourcesAsset<T>).OnInstantiation();
                        Debug.LogWarningFormat("Created instance of {0}", AssetName);
#else
                    Debug.LogErrorFormat("Asset {0} not found!", AssetName);
#endif
                    }
                    else
                    {
                        if (_instance is SingletonResourcesAsset<T>)
                            (_instance as SingletonResourcesAsset<T>).OnInstantiation();
                    }
                }
                return _instance;
            }
        }

        protected virtual void OnInstantiation() { }

        private static String AssetName { get { return typeof(T).Name; } }

#if UNITY_EDITOR
        public static String GetAssetPath()
        {
            var dataPath = UnityEngine.Application.dataPath;
            if (!dataPath.EndsWith("/"))
            {
                dataPath += "/";
            }
            dataPath = String.Format("{0}Resources/{1}.asset", dataPath, AssetName);
            var index = dataPath.IndexOf("Assets", StringComparison.InvariantCulture);
            return dataPath.Substring(index);
        }

#endif
    }
}