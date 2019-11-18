using UnityEngine;

namespace Assets.ZplaySDK.Scripts.Extentions
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var objects = FindObjectsOfType<T>();
                    if (objects.Length > 0)
                    {
                        foreach (T obj in objects)
                        {
                            if ((_instance == null) && (obj != null))
                                _instance = objects[0];
                            else
                                Destroy(obj);
                        }
                    }
                    else
                    {
                        var instanceHolder = new GameObject(typeof(T).Name);
                        _instance = instanceHolder.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        private Transform GetRootTransform(Transform targetTransform)
        {
            return targetTransform.parent == null ? targetTransform : GetRootTransform(targetTransform.parent);
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(GetRootTransform(transform));
            }
            else if (_instance != this)
            {
                Destroy(this);
            }
        }

        protected virtual void OnDestroy()
        {
        }
    }
}
