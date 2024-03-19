using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationPointController : MonoBehaviour, IPrepare
{
    public static LocationPointController instance;
    private LocationPointsWrapper pointsData;
    private List<LocationPoint> locationPoints = new List<LocationPoint>();

    public Dictionary<string, GameObject> locationPointPrefabs = new Dictionary<string, GameObject>();
    public GameObject test;

    private void Awake()
    {
        //
        locationPointPrefabs.Add("INFO", test);
        //
        if (instance == null)
        {
            instance = this;
        }
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        try
        {
            string key = $"{GeoLocationManager.country}_{GeoLocationManager.city}";

            PlayFabTitleData.GetTitleData(key, value =>
            {
                string jsonString = value;
                string modifiedJsonString = "{\"points\":" + jsonString + "}";
                pointsData = JsonUtility.FromJson<LocationPointsWrapper>(modifiedJsonString);

                InstantiateLocationPoints();

                onComplete?.Invoke(true, null);
            },
            () =>
            {
                onComplete?.Invoke(true, "No data available");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Prepare LocationPointController result: {false} {ex.Message}");
            onComplete?.Invoke(false, ex.Message);
        }

        yield break;
    }

    private void InstantiateLocationPoints()
    {
        foreach (var pointData in pointsData.points)
        {
            var newPoint = Instantiate(locationPointPrefabs[pointData.type]);
            newPoint.GetComponent<LocationPoint>().SetData(pointData);
        }
    }
}

