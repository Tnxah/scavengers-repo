using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationPointController : MonoBehaviour, IPrepare
{
    public static LocationPointController instance;
    private List<LocationPointData> pointsData;
    private List<LocationPoint> locationPoints = new List<LocationPoint>();
    private bool hasLocationPoints;

    public Dictionary<string, GameObject> locationPointPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
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
                pointsData = DeserializeJsonToList<LocationPointData>(jsonString);

                hasLocationPoints = true;

                ReadPrefabs();

                InstantiateLocationPoints();

                StartCoroutine(TimeManager());

                onComplete?.Invoke(true, null);
            },
            () =>
            {
                hasLocationPoints = false;

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
        if (!hasLocationPoints) return;

        foreach (var pointData in pointsData)
        {
            var newPoint = Instantiate(locationPointPrefabs[pointData.type]).GetComponent<LocationPoint>();
            newPoint.SetData(pointData);
            newPoint.Hide();
            locationPoints.Add(newPoint);
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

    private void ReadPrefabs()
    {
        var prefabs = Resources.LoadAll<GameObject>("LocationPoints").ToList();
        foreach (var prefab in prefabs)
        {
            locationPointPrefabs.Add(prefab.name, prefab);
        }
        prefabs = null;
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

