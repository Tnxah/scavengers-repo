using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcePointFactory
{
    private float globalSeed;
    private Dictionary<string, GameObject> resourcePointPrefabs;
    private const float scale = 0.05f;

    public ResourcePointFactory(float globalSeed)
    {
        this.globalSeed = globalSeed;
        resourcePointPrefabs = new Dictionary<string, GameObject>();
        LoadPrefabs();
    }

    public string DetermineResourceType(Vector2Int cell)
    {
        float noiseValue = Mathf.PerlinNoise(((float)cell.x + globalSeed) * scale, ((float)cell.y + globalSeed) * scale);
        float normalizedValue = (noiseValue * 1000) % 100;

        if (normalizedValue < 50) return "EMPTY"; // 0-49
        else if (normalizedValue < 72) return "COAL"; // 50-71
        else if (normalizedValue < 87) return "METAL"; // 72-86
        else if (normalizedValue < 97) return "WATER"; // 87-96
        else return "RUINS"; // 97-99
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
