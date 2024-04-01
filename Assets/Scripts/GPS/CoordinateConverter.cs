using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoordinateConverter
{
    private static float referenceLatitude;
    private static float referenceLongitude; 
    private static readonly float EarthRadius = 6378137; // Earth’s mean radius in meter

    public static bool isReady;

    public static void SetReferencePoint(float latitude, float longitude)
    {
        referenceLatitude = latitude;
        referenceLongitude = longitude;

        isReady = true;
    }

    public static Vector3 GPSToGamePosition(float latitude, float longitude)
    {
        float distanceX = GetEastWestDistance(referenceLongitude, referenceLatitude, longitude); // East-West distance
        float distanceZ = GetNorthSouthDistance(referenceLatitude, latitude); // North-South distance

        if (longitude < referenceLongitude) distanceX *= -1;
        if (latitude < referenceLatitude) distanceZ *= -1;

        return new Vector3(distanceX, 0, distanceZ);
    }

    public static (float latitude, float longitude) GameToGPSPosition(Vector3 gamePosition)
    {
        float latitude = GetLatitudeFromDistance(referenceLatitude, gamePosition.x);
        float longitude = GetLongitudeFromDistance(referenceLongitude, referenceLatitude, gamePosition.z);

        return (latitude, longitude);
    }

    private static float GetNorthSouthDistance(float lat1, float lat2)
    {
        var dLat = ToRadians(lat2 - lat1);
        var a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2);
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return EarthRadius * c;
    }

    private static float GetEastWestDistance(float lon1, float referenceLatitude, float lon2)
    {
        var dLong = ToRadians(lon2 - lon1);
        var a = Mathf.Cos(ToRadians(referenceLatitude)) * Mathf.Cos(ToRadians(referenceLatitude)) *
                Mathf.Sin(dLong / 2) * Mathf.Sin(dLong / 2);
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return EarthRadius * c;
    }

    private static float GetLatitudeFromDistance(float referenceLatitude, float distance)
    {
        float deltaLatitude = distance / EarthRadius;
        return ToDegrees(ToRadians(referenceLatitude) + deltaLatitude);
    }

    private static float GetLongitudeFromDistance(float referenceLongitude, float referenceLatitude, float distance)
    {
        float deltaLongitude = distance / (EarthRadius * Mathf.Cos(ToRadians(referenceLatitude)));
        return ToDegrees(ToRadians(referenceLongitude) + deltaLongitude);
    }

    private static float ToRadians(float angle)
    {
        return angle * Mathf.PI / 180;
    }

    private static float ToDegrees(float angle)
    {
        return angle * 180 / Mathf.PI;
    }
}


