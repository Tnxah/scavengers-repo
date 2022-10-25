using Google.Maps;
using Google.Maps.Examples.Shared;
using Google.Maps.Coord;
using Google.Maps.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MapsService))]
public class MapPart : MonoBehaviour
{
    MapsService mapsService;
    
    LatLng LatLng = new LatLng(0, 0);

    private bool _load = false;

    public float MapSize;

    public Vector2 index;

    BoxCollider boxCollider;

    void Start()
    {
        mapsService = GetComponent<MapsService>();
        StartCoroutine(MapLoading());
        boxCollider = GetComponent<BoxCollider>();
    }
    public void SetLatLan(Vector2 coords)
    {
        LatLng = new LatLng(coords.x, coords.y);
        _load = true;
    }

    public void SetIndex(int x, int y)
    {
        index = new Vector2(x, y);

        gameObject.name = index.ToString();

        float X = x * MapSize;
        float Y = y * MapSize;
        
        //transform.position = new Vector3(X, 0, Y);

        Vector2 coords = CoordinateRecounter.RecountReverse(X, Y);

        transform.position = CoordinateRecounter.Recount(coords.x, coords.y);

        SetLatLan(coords);
    }

    IEnumerator MapLoading()
    {
        yield return new WaitUntil(() => _load);
        //print("Map part loading started in " + LatLng);
        mapsService.InitFloatingOrigin(LatLng);
        mapsService.Events.MapEvents.Loaded.AddListener(OnLoaded);
        mapsService.LoadMap(ExampleDefaults.DefaultBounds, ExampleDefaults.DefaultGameObjectOptions);
    }

    public void SetColliderSize()
    {
        boxCollider.size = new Vector3(MapSize, 80, MapSize);
    }

    public void OnLoaded(MapLoadedArgs args)
    {
        SetColliderSize();
    }
}
