using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour, IPrepare
{
    private ResourcePointFactory resourcePointFactory;
    private const int MapSeed = 1204;

    public static MapController instance;

    private static List<Vector2Int> placedResources = new List<Vector2Int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        yield return new WaitUntil(() => GPSController.isLocationServiceEnabled);

        resourcePointFactory = new ResourcePointFactory(MapSeed);
        GridManager.Seed = MapSeed;

        StartCoroutine(ManageResources());

        onComplete?.Invoke(true, null);
    }


    private IEnumerator ManageResources()
    {
        while (true)
        {
            var cell = GridManager.GPSToGrid(GPSController.latitude, GPSController.longitude);
            
            if (!placedResources.Contains(cell))
            {
                //Debug.Log($"{cell.x},{cell.y}");
                //resourcePointFactory.DebugResourceList(cell, 15);
                if (resourcePointFactory.CreateResourcePoint(cell) != null)
                {
                    placedResources.Add(cell);
                }
            }
            yield return new WaitForSeconds(10);
        }
    }
}
