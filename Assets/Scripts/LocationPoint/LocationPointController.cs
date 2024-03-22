using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationPointController : MonoBehaviour, IPrepare
{
    public static LocationPointController instance;

    [SerializeField]
    private LocationPointFactory locationPointFactory;

    private List<LocationPointData> pointsData;
    private List<LocationPoint> locationPoints = new List<LocationPoint>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        locationPointFactory = new LocationPointFactory();
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        try
        {
            string key = $"{GeoLocationManager.country}_{GeoLocationManager.city}";

            PlayFabTitleData.GetTitleData(key, value =>
            {
                string jsonString = value;
                pointsData = DeserializeJsonToList<LocationPointData>(jsonString);

                InstantiateLocationPoints();

                StartCoroutine(TimeManager());

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
            onComplete?.Invoke(true, ex.Message);
        }

        yield break;
    }

    private static List<T> DeserializeJsonToList<T>(string jsonArray)
    {
        string newJson = "{\"list\":" + jsonArray + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.list;
    }
    private void SortByDistance()
    {
        locationPoints = locationPoints.OrderBy(x => x.DistanceToPlayer()).ToList();
    }

    private void InstantiateLocationPoints()
    {
        foreach (var pointData in pointsData)
        {
            LocationPoint newPoint = locationPointFactory.CreateLocationPoint(pointData);
            if (newPoint != null)
            {
                newPoint.SetData(pointData);
                locationPoints.Add(newPoint);
            }
        }
        SortByDistance();
        ManageLocationPoints();
    }

    private void ManageLocationPoints()
    {
        foreach (var locationPoint in locationPoints)
        {
            if (locationPoint.DistanceToPlayer() < 1000f)
            {
                locationPoint.Unhide();
                continue;
            }
            locationPoint.Hide();
        }
    }

    private IEnumerator TimeManager()
    {
        while (locationPoints.Count > 0)
        {
            yield return new WaitForSeconds(10);
            ManageLocationPoints();
        }
    }
}

