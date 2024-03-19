using System;
using UnityEngine;

[Serializable]
public class GeoLocationData
{
    public string country_code;
    public string city;

    public static GeoLocationData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GeoLocationData>(jsonString);
    }
}
