# AndroidRuntimPermissions plugin for Unity

* This SDK contains Android dynamic request sensitive permissions
* To add this sdk you need to fill in the permissions from your Assets/ Plugins/Android/AndroidMainfiest.xml:
__<uses-permission android:name="android.permission.READ_PHONE_STATE" />__   
__<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />__  
__<uses-permission android:name="android.permission. ACCESS_COARSE_LOCATION" />__  
* This sdk only contains the above three permissions, namely, obtaining phone permissions, obtaining targeting permissions, and obtaining read or write file permissions.
* If you need to add other sensitive permissions, please contact the zplay developer.

## To related function, call:   
if (!RuntimePermissionsHandler.IsPermissionGranted(AndroidPermission.READ_PHONE_STATE))
{
		// Permission is never asked, then jump directly to the current application settings permission interface
            RuntimePermissionsHandler.ShowAppSettings();
}
 else
{
        // One-on-one permission, single permission pops up
        RuntimePermissionsHandler.GrantPermission(AndroidPermission.READ_PHONE_STATE, (permission, result) =>
        {
             if (permission == AndroidPermission.READ_PHONE_STATE && result)
             {
                 // Authorized success
             }
        });
        // Multiple permissions pop up at a time
        AndroidPermission[] permissionsArr = { AndroidPermission.ACCESS_COARSE_LOCATION,AndroidPermission.WRITE_EXTERNAL_STORAGE};
        RuntimePermissionsHandler.GrantPermissions(permissionsArr, result =>
        {
             if (result)
             {
                 // Authorized success
             }
        });
}









