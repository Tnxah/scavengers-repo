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

    public string DetermineResourceType(Vector2Int cell)
    {
        float noiseValue = Mathf.PerlinNoise((cell.x) * 0.9f, (cell.y + globalSeed) * 0.9f);

        int normalizedValue = (int)(noiseValue * 1000 % 100);

        if (normalizedValue < 35) return "EMPTY"; // 0-34
        else if (normalizedValue < 55) return "COAL"; // 35-54
        else if (normalizedValue < 70) return "METAL"; // 55-69
        else if (normalizedValue < 85) return "WATER"; // 70-84
        else return "RUINS"; // 85-99
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
        string resourceType = DetermineResourceType(cell);

        if (resourceType.Equals("EMPTY")) return null;

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
