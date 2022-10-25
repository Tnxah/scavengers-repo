using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateRecounter
{
    static Vector3 position = Vector3.zero;
    static double lat, lon;

    static Vector3 iniRef;

    public static Vector3 Recount(float lat, float lon)
    {
        iniRef = GPS.instance.iniRef;

        position.x = (float)(((lon * 20037508.34) / 18000) - iniRef.x);
        position.z = (float)(((Mathf.Log(Mathf.Tan((90 + lat) * Mathf.PI / 360)) / (Mathf.PI / 180)) * 1113.19490777778) - iniRef.z);
        
        //sizing
        position.x *= 60.5f;
        position.z *= 60.5f;

        return new Vector3(position.x, 0, position.z);
    }

    public static Vector2 RecountReverse(float x, float z)
    {
        x /= 60.5f;
        z /= 60.5f;

        iniRef = GPS.instance.iniRef;

        lon = (float)(((x + iniRef.x) * 100) * 180 / 20037508.34);
        lat = (Mathf.Atan(Mathf.Pow((float)Math.E, (float)((z + iniRef.z) * (Mathf.PI / 180) / 1113.19490777778))) / Mathf.PI * 360) - 90;

        return new Vector2(((float)lat), ((float)lon));
    }

}