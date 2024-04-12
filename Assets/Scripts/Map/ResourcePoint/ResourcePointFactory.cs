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

        if (normalizedValue < 30) return "EMPTY"; // 0-29
        else if (normalizedValue < 55) return "COAL"; // 30-54
        else if (normalizedValue < 75) return "METAL"; // 55-74
        else if (normalizedValue < 90) return "WATER"; // 75-89
        else return "RUINS"; // 90-99
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
