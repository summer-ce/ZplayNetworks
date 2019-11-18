using System;

namespace Assets.ZplaySDK.Scripts.Extentions
{
    public static class ExtentionMethods
    {
        #region ActionsExtentions
        public static void SafeInvoke(this Action action)
        {
            if (action == null)
                return;

            action();
        }

        public static void SafeInvoke<T>(this Action<T> action, T arg1)
        {
            if (action == null)
                return;

            action(arg1);
        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (action == null)
                return;

            action(arg1, arg2);
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (action == null)
                return;

            action(arg1, arg2, arg3);
        }

        #endregion
    }
}