using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class GPSController : MonoBehaviour, IPrepare
{
    public static float latitude;
    public static float longitude;
    public static bool isLocationServiceEnabled;

    public float lat, lon;

    private bool isPermissionGranted;
    private const string locationPermission = Permission.FineLocation;

    private const float RefreshTime = 0.5f;
    private static float lastRrefreshTime;

    public static GPSController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        latitude = lat;
        longitude = lon;
        return;
#endif
        if (Input.location.status == LocationServiceStatus.Running/* && Time.time - lastRrefreshTime >= RefreshTime*/)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            //lastRrefreshTime = Time.time;
        }
    }

    private void GPSFunctionManage(Action<bool, string> onComplete) {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            isLocationServiceEnabled = false;
            onComplete?.Invoke(false, "User has not enabled GPS");

            //ask player to ensble GPS //TODO
        }
    }

    private void PermissionManage(Action<bool, string> onComplete)
    {
        Action<string> onPermissionGranted = permission =>
        {
            isPermissionGranted = true;
        };

        Action<string> onPermissionDenied = permission =>
        {
            onComplete?.Invoke(false, $"{permission} permission denied by player");
        };

        PermissionManager.GetPermission(locationPermission, onPermissionGranted, onPermissionDenied);
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
#if UNITY_EDITOR
        CoordinateConverter.SetReferencePoint(latitude, longitude);
        isLocationServiceEnabled = true;
        onComplete?.Invoke(true, null);
        yield break;
#endif

        GPSFunctionManage(onComplete);
        //yield return new WaitUntil(() => Input.location.isEnabledByUser);

        PermissionManage(onComplete);
        yield return new WaitUntil(() => isPermissionGranted);

        // Start service before querying location
        Input.location.Start(1f, 0.5f);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            isLocationServiceEnabled = false;
            onComplete?.Invoke(false, "Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            isLocationServiceEnabled = false;
            onComplete?.Invoke(false, "Unable to determine device location");
            yield break;
        }

        yield return new WaitUntil(() => Input.location.status == LocationServiceStatus.Running);

        try
        {
            // Access granted and location value could be retrieved
            CoordinateConverter.SetReferencePoint(latitude, longitude);

            isLocationServiceEnabled = true;

            // If successful
            Console.WriteLine($"Prepare GPS Controller result: {isLocationServiceEnabled} {null}");
            onComplete?.Invoke(true, null);
        }
        catch (Exception ex)
        {
            // On error
            Console.WriteLine($"Prepare GPS Controller result: {false} {ex.Message}");
            onComplete?.Invoke(false, ex.Message);
        }

        yield break;
    }
}
