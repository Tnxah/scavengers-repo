using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour, IPrepare
{
    private ResourcePointFactory resourcePointFactory;
    private float MapSeed;

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
        MapSeed = TitleInfo.MapSeed;

        yield return new WaitUntil(() => GPSController.isLocationServiceEnabled);

        resourcePointFactory = new ResourcePointFactory(MapSeed);
        GridManager.Seed = MapSeed;

        StartCoroutine(ManageResources());

        onComplete?.Invoke(true, null);
    }


    private IEnumerator ManageResources()
    {
        resourcePointFactory.DebugResourceList(GridManager.GPSToGrid(GPSController.latitude, GPSController.longitude), 15);
        while (true)
        {
            var cell = GridManager.GPSToGrid(GPSController.latitude, GPSController.longitude);
            
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var newCell = new Vector2Int(cell.x + i, cell.y + j);
                    if (!placedResources.Contains(newCell) && resourcePointFactory.CreateResourcePoint(newCell) != null)
                    {
                        placedResources.Add(newCell);
                    }
                }
            }
            
            yield return new WaitForSeconds(60);
        }
    }
}
