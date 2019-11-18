using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Assets.ZplaySDK.Scripts.Services
{
    public static class Storage
    {
        private static readonly Dictionary<String, Boolean> _cachedBooleans = new Dictionary<String, Boolean>();

        private static void Set(String key, Byte[] byteRepresentation)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(GetHashData(key, byteRepresentation));
                var resultArray = Combine(hash, byteRepresentation);
                Swap(resultArray, 0, resultArray.Length - 1);
                PlayerPrefs.SetString(key, Convert.ToBase64String(resultArray));
            }
        }

        private static Byte[] Get(String key)
        {
            var savedData = PlayerPrefs.GetString(key, null);
            if (!String.IsNullOrEmpty(savedData) && Regex.IsMatch(savedData, "^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$", RegexOptions.None))
            {
                var storedBytes = Convert.FromBase64String(savedData);
                if (storedBytes.Length < 16)
                {
                    return null;
                }
                Swap(storedBytes, 0, storedBytes.Length - 1);
                var valueByteRepresentation = new Byte[storedBytes.Length - 16];
                Buffer.BlockCopy(storedBytes, 16, valueByteRepresentation, 0, storedBytes.Length - 16);
                using (var md5 = MD5.Create())
                {
                    if (IsValidHash(storedBytes, md5.ComputeHash(GetHashData(key, valueByteRepresentation))))
                    {
                        return valueByteRepresentation;
                    }
                }
            }
            return null;
        }

        public static void Set(String key, Int32 value)
        {
            Set(key, BitConverter.GetBytes(value));
        }

        public static Int32 GetInt(String key, Int32 defaultValue = 0)
        {
            var valueByteRepresentation = Get(key);
            return valueByteRepresentation == null ? defaultValue : BitConverter.ToInt32(valueByteRepresentation, 0);
        }

        public static void Set(String key, Boolean value)
        {
            if (_cachedBooleans.ContainsKey(key))
            {
                _cachedBooleans[key] = value;
            }
            Set(key, BitConverter.GetBytes(value));
        }

        public static Boolean GetBoolean(String key, Boolean defaultValue = false, Boolean useCachedValue = false)
        {
            Byte[] valueByteRepresentation;
            if (useCachedValue)
            {
                if (_cachedBooleans.ContainsKey(key))
                {
                    return _cachedBooleans[key];
                }
                valueByteRepresentation = Get(key);
                _cachedBooleans[key] = valueByteRepresentation == null ? defaultValue : BitConverter.ToBoolean(valueByteRepresentation, 0);
                return _cachedBooleans[key];
            }
            valueByteRepresentation = Get(key);
            return valueByteRepresentation == null ? defaultValue : BitConverter.ToBoolean(valueByteRepresentation, 0);
        }

        public static void Set(String key, Single value)
        {
            Set(key, BitConverter.GetBytes(value));
        }

        public static Single GetSingle(String key, Single defaultValue = 0f)
        {
            var valueByteRepresentation = Get(key);
            return valueByteRepresentation == null ? defaultValue : BitConverter.ToSingle(valueByteRepresentation, 0);
        }

        public static void Set(String key, DateTime value)
        {
            Set(key, BitConverter.GetBytes(value.ToBinary()));
        }

        public static DateTime GetDateTime(String key, DateTime defaultValue = default(DateTime))
        {
            var valueByteRepresentation = Get(key);
            return valueByteRepresentation == null ? defaultValue : DateTime.FromBinary(BitConverter.ToInt64(valueByteRepresentation, 0));
        }

        /// <summary>
        /// Sets string value to the storage. Using null as value is forbidden
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(String key, String value)
        {
            if (value == null)
            {
                throw new Exception("Using null is forbidden");
            }
            Set(key, Encoding.UTF8.GetBytes(value));
        }

        public static String GetString(String key, String defaultValue = "")
        {
            var valueByteRepresentation = Get(key);
            return valueByteRepresentation == null ? defaultValue : Encoding.UTF8.GetString(valueByteRepresentation);
        }

        public static Boolean HasKey(String key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public static void DeleteKey(String key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        private static Boolean IsValidHash(Byte[] dataArray, Byte[] hash)
        {
            for (var i = 0; i < hash.Length; ++i)
            {
                if (i >= dataArray.Length || dataArray[i] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static Byte[] Combine(Byte[] first, Byte[] second)
        {
            var result = new Byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, result, 0, first.Length);
            Buffer.BlockCopy(second, 0, result, first.Length, second.Length);
            return result;
        }

        private static void Swap(Byte[] array, Int32 i, Int32 j)
        {
            var tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }

        private static Byte[] GetHashData(String key, Byte[] byteValueRepresentation)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            Array.Reverse(keyBytes);
            return Combine(keyBytes, byteValueRepresentation);
        }
    }
}
