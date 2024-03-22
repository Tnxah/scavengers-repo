using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationPointFactory
{
    private Dictionary<string, GameObject> locationPointPrefabs;
    public LocationPointFactory()
    {
        locationPointPrefabs = new Dictionary<string, GameObject>();
        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        var prefabs = Resources.LoadAll<GameObject>("LocationPoints").ToList();
        foreach (var prefab in prefabs)
        {
            locationPointPrefabs.Add(prefab.name, prefab);
        }
    }

    public LocationPoint CreateLocationPoint(ILocationPointData data)
    {
        if (locationPointPrefabs.TryGetValue(data.Type, out GameObject prefab))
        {
            GameObject instance = GameObject.Instantiate(prefab);
            LocationPoint locationPoint = instance.GetComponent<LocationPoint>();
            if (locationPoint != null)
            {
                locationPoint.SetData(data);
                return locationPoint;
            }
        }
        return null;
    }
}
