using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcePointFactory
{
    private int globalSeed;
    private Dictionary<string, GameObject> resourcePointPrefabs;

    public ResourcePointFactory(int globalSeed)
    {
        this.globalSeed = globalSeed;
        resourcePointPrefabs = new Dictionary<string, GameObject>();
        LoadPrefabs();
    }

    public string DetermineResourceType(int cellHash)
    {
        if (cellHash % 3 == 0) return "METAL";
        if (cellHash % 3 == 1) return "COAL";
        if (cellHash % 3 == 2) return "WATER";
        return "RUINS";
    }

    public int GenerateGridCellHash(Vector2Int cell)
    {
        return cell.x.GetHashCode() ^ cell.y.GetHashCode() ^ globalSeed;
    }

    private void LoadPrefabs()
    {
        var prefabs = Resources.LoadAll<GameObject>("ResourcePoints").ToList();
        foreach (var prefab in prefabs)
        {
            resourcePointPrefabs.Add(prefab.name, prefab);
        }
    }

    public ResourcePoint CreateResourcePoint(Vector2Int cell)
    {
        int hash = GenerateGridCellHash(cell);

        string resourceType = DetermineResourceType(hash);

        if (resourcePointPrefabs.TryGetValue(resourceType, out GameObject prefab))
        {
            GameObject instance = GameObject.Instantiate(prefab);

            ResourcePoint resourcePoint;
            if (instance.TryGetComponent<ResourcePoint>(out resourcePoint))
            {
                resourcePoint.SetData(cell, resourceType);
                return resourcePoint;
            }
        }
        return null;
    }
}
