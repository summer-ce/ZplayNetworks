using System;

namespace Assets.ZplaySDK.Scripts.Services
{
    public static class GameData
    {
        public static class SampleDemo
        {
            public static DateTime NextTime
            {
                get { return Storage.GetDateTime("NextTime", DateTime.Now.AddSeconds(-1)); }
                set { Storage.Set("NextTime", value); }
            }
            private static Boolean? _isOpenTime;
            public static Boolean OpenTime
            {
                get { return _isOpenTime ?? (_isOpenTime = PlayerPrefs.GetBoolean("OpenTime", true)).Value; }
                set { _isOpenTime = true; PlayerPrefs.SetBoolean("OpenTime", value); }
            }
        }
        public static class Permissions
        {
            public static Boolean WasPermissionRequested(AndroidPermission permission)
            {
                return PlayerPrefs.GetBoolean("WAS_REQUESTED_" + permission);
            }

            public static void SetPermissionRequested(AndroidPermission permission)
            {
                PlayerPrefs.SetBoolean("WAS_REQUESTED_" + permission, true);
            }
        }
    }
}
