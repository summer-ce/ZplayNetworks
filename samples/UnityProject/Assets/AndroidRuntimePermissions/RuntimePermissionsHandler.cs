#if UNITY_ANDROID
using System;
using System.Linq;
using UnityEngine;
using Assets.ZplaySDK.Scripts.Extentions;
using Assets.ZplaySDK.Scripts.Services;

public class RuntimePermissionsHandler : MonoSingleton<RuntimePermissionsHandler>
{

    private const String PluginListenerName = "PermissionRequesterListener"; // must match UnitySendMessage call in Java

    private const String PermissionGranted = "PERMISSION_GRANTED"; // must match Java
    private const String PermissionDenied = "PERMISSION_DENIED"; // must match Java

    private static Action<AndroidPermission, Boolean> _permissionGrantedCallback;
    private static Action<Boolean> _requestedPermissionsGrantedCallback;
    private AndroidJavaClass _permissionGranterClass;

    protected override void Awake()
    {
        base.Awake();
        _permissionGranterClass = new AndroidJavaClass("com.zplay.runtimepermissionshandler.PermissionGranter");
        if (name != PluginListenerName)
        {
            name = PluginListenerName;
        }
    }

    public void Initialize()
    {
    }

    private void SinglePermissionRequestCallbackInternal(String result)
    {
        Debug.Log("SinglePermissionRequestCallbackInternal: " + result);
        if (!String.IsNullOrEmpty(result))
        {
            var splittedResult = result.Split(':');
            if (splittedResult.Length == 2)
            {
                var permissionIndex = 0;
                if (Int32.TryParse(splittedResult[0], out permissionIndex))
                {
                    _permissionGrantedCallback.SafeInvoke((AndroidPermission)permissionIndex, splittedResult[1] == PermissionGranted);
                }
            }
        }
    }

    private void MultiPermissionRequestCallbackInternal(String result)
    {
        Debug.Log("MultiPermissionRequestCallbackInternal: " + result);
        if (!String.IsNullOrEmpty(result))
        {
            _requestedPermissionsGrantedCallback.SafeInvoke(result == PermissionGranted);
        }
    }

    /// <summary>
    /// Check if permissions are allowed
    /// </summary>
    /// <param name="permission"></param>
    /// <returns></returns>
    public static Boolean IsPermissionGranted(AndroidPermission permission)
    {
        var result = Instance._permissionGranterClass.CallStatic<Boolean>("isPermissionGranted", (Int32)permission);
        Debug.Log(String.Format("Is permission permission {0} granted: {1}", permission, result));
        return result;
    }

    /// <summary>
    /// The popup specifies the permissions that need to be specified
    /// </summary>
    /// <param name="permission"></param>
    /// <returns></returns>
    public static Boolean ShowPermissionsExplanation(AndroidPermission permission)
    {
        return Instance._permissionGranterClass.CallStatic<Boolean>("showPermissionExplanation", (Int32)permission);
    }

    /// <summary>
    /// Jump to the current application Settings permission interface
    /// </summary>
    public static void ShowAppSettings()
    {
        Instance._permissionGranterClass.CallStatic("showAppSettings");
    }

    /// <summary>
    /// The popup specifies the permissions that need to be specified,And returns whether permission is allowed
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="callback"></param>
    public static void GrantPermission(AndroidPermission permission, Action<AndroidPermission, Boolean> callback)
    {
        GameData.Permissions.SetPermissionRequested(permission);
        _permissionGrantedCallback = callback;
        Instance._permissionGranterClass.CallStatic("grantPermission", (Int32)permission);
    }

    /// <summary>
    /// The popup specifies the permissions that need to be specified,And returns whether permission is allowed
    /// </summary>
    /// <param name="permissions"></param>
    /// <param name="callback"></param>
    public static void GrantPermissions(AndroidPermission[] permissions, Action<Boolean> callback)
    {
        foreach (var permission in permissions)
        {
            GameData.Permissions.SetPermissionRequested(permission);
        }
        _requestedPermissionsGrantedCallback = callback;
        Instance._permissionGranterClass.CallStatic("grantPermissions", permissions.Select(p => (Int32)p).ToArray());
    }

    /// <summary>
    /// Signature comparison
    /// </summary>
    /// <returns></returns>
    public static String[] GetSignatures()
    {
        var result = Instance._permissionGranterClass.CallStatic<String[]>("requestPermissions"); //This method actually retrieves build signatures
        return result;
    }
}

public static class CheckPermissions
{
    public static Boolean Check_Read_Phone_State_Permission
    {
        get
        {
            if (GameData.Permissions.WasPermissionRequested(AndroidPermission.READ_PHONE_STATE)
                && !RuntimePermissionsHandler.IsPermissionGranted(AndroidPermission.READ_PHONE_STATE)
                && !RuntimePermissionsHandler.ShowPermissionsExplanation(AndroidPermission.READ_PHONE_STATE))
            {
                return true;
            }
            return false;
        }
    }
    public static Boolean Check_Write_External_Storage_Prmission
    {
        get
        {
            if (GameData.Permissions.WasPermissionRequested(AndroidPermission.WRITE_EXTERNAL_STORAGE)
                && !RuntimePermissionsHandler.IsPermissionGranted(AndroidPermission.WRITE_EXTERNAL_STORAGE)
                && !RuntimePermissionsHandler.ShowPermissionsExplanation(AndroidPermission.WRITE_EXTERNAL_STORAGE))
            {
                return true;
            }
            return false;
        }
    }
    public static Boolean Check_Access_Coarse_Location
    {
        get
        {
            if (GameData.Permissions.WasPermissionRequested(AndroidPermission.ACCESS_COARSE_LOCATION)
                && !RuntimePermissionsHandler.IsPermissionGranted(AndroidPermission.ACCESS_COARSE_LOCATION)
                && !RuntimePermissionsHandler.ShowPermissionsExplanation(AndroidPermission.ACCESS_COARSE_LOCATION))
            {
                return true;
            }
            return false;
        }
    }
}
#endif

