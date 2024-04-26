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

        if (normalizedValue < 50) return "EMPTY"; // 0-49
        else if (normalizedValue < 70) return "COAL"; // 50-69
        else if (normalizedValue < 85) return "METAL"; // 70-84
        else if (normalizedValue < 95) return "WATER"; // 85-94
        else return "RUINS"; // 95-99
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

    public void DebugResourceList(Vector2Int cell, int halfSize)
    {
        string ResourceList = "";

        for (int y = cell.y - halfSize; y < cell.y + halfSize; y++)
        {
            for (int x = cell.x - halfSize; x < cell.x + halfSize; x++)
            {
                ResourceList += DetermineResourceType(new Vector2Int(x, y))+ $":{x},{y} ";
            }
            ResourceList += "\n";
        }
        Debug.Log(ResourceList);
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
