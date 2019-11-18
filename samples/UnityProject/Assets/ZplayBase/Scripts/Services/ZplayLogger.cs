using UnityEngine;
using System;

namespace Assets.ZplaySDK.Scripts.Services
{
    public class ZplayLogger
    {
        private static bool bDebug = false;
        private const String TAG = "ZplayLogger ：";

        public static void SetDebug(bool bValue)
        {
            bDebug = bValue;
        }

        public static void Log(string info)
        {
            if (bDebug == true)
            {
                Debug.Log(string.Format("{0}{1}", TAG, info));
            }
        }

        public static void Log(string info, UnityEngine.Object obj)
        {
            if (bDebug == true)
            {
                Debug.Log(string.Format("{0}{1}", TAG, info), obj);
            }
        }

        public static void LogWarning(string info, UnityEngine.Object obj)
        {
            if (bDebug == true)
            {
                Debug.LogWarning(string.Format("{0}{1}", TAG, info), obj);
            }
        }

        public static void LogWarning(string info)
        {
            if (bDebug == true)
            {
                Debug.LogWarning(string.Format("{0}{1}", TAG, info));
            }
        }

        public static void LogError(string info)
        {
            if (bDebug == true)
            {
                Debug.LogError(string.Format("{0}{1}", TAG, info));
            }
        }

        public static void LogError(string info, UnityEngine.Object obj)
        {
            if (bDebug == true)
            {
                Debug.LogError(string.Format("{0}{1}", TAG, info), obj);
            }
        }

        public static void LogException(Exception info)
        {
            if (bDebug == true)
            {
                Debug.LogException(info);
            }
        }

        public static void LogException(Exception info, UnityEngine.Object obj)
        {
            if (bDebug == true)
            {
                Debug.LogException(info, obj);
            }
        }
    }
}
