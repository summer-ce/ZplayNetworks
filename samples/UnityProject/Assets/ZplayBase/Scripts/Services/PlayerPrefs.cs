using System;
using UnityPrefs = UnityEngine.PlayerPrefs;

namespace Assets.ZplaySDK.Scripts.Services
{
    public static class PlayerPrefs
    {
        public static Boolean HasKey(String key)
        {
            return UnityPrefs.HasKey(key);
        }

        public static void SetInt(String key, Int32 value)
        {
            UnityPrefs.SetInt(key, value);
        }

        public static Int32 GetInt(String key, Int32 defaultValue = 0)
        {
            return UnityPrefs.GetInt(key, defaultValue);
        }

        public static void SetBoolean(String key, Boolean value)
        {
            UnityPrefs.SetInt(key, value ? 1 : 0);
        }

        public static Boolean GetBoolean(String key, Boolean defaultValue = false)
        {
            return HasKey(key)
                ? UnityPrefs.GetInt(key, 0) == 1
                : defaultValue;
        }

        public static void SetFloat(String key, Single value)
        {
            UnityPrefs.SetFloat(key, value);
        }

        public static Single GetFloat(String key, Single defaultValue = 0f)
        {
            return UnityPrefs.GetFloat(key, defaultValue);
        }

        public static void SetString(String key, String value)
        {
            UnityPrefs.SetString(key, value);
        }

        public static String GetString(String key, String defaultValue = "")
        {
            return UnityPrefs.GetString(key, defaultValue);
        }

        public static void SetDateTime(String key, DateTime value)
        {
            SetString(key, Convert.ToBase64String(BitConverter.GetBytes(value.ToBinary())));
        }

        public static DateTime GetDateTime(String key, DateTime defaultValue = default(DateTime))
        {
            var storedString = GetString(key, null);
            return String.IsNullOrEmpty(storedString) ? defaultValue : DateTime.FromBinary(BitConverter.ToInt64(Convert.FromBase64String(storedString), 0));
        }

        public static void DeleteKey(String key)
        {
            UnityPrefs.DeleteKey(key);
        }

        public static void DeleteAll()
        {
            UnityPrefs.DeleteAll();
        }

        public static void Save()
        {
            UnityPrefs.Save();
        }
    }
}
