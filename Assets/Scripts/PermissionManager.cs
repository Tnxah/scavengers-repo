using System;
using UnityEngine.Android;

public class PermissionManager
{
    public static void GetPermission(string permission, Action<string> onSuccess, Action<string> onFail)
    {
        if (Permission.HasUserAuthorizedPermission(permission))
        {
            onSuccess?.Invoke(permission);
        }
        else
        {
            var callback = new PermissionCallbacks();
            callback.PermissionDenied += permissionName => onFail?.Invoke(permissionName);
            callback.PermissionGranted += permissionName => onSuccess?.Invoke(permissionName);
            callback.PermissionDeniedAndDontAskAgain += permissionName => onFail?.Invoke(permissionName);

            Permission.RequestUserPermission(permission, callback);
        }
    }
}
