
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static Dictionary<string, GameObject> _loadedParts = new Dictionary<string, GameObject>();

    private int X = 0;
    private int Y = 0;

    private float MapSize = 694f;

    private GameObject MapPartPrefab;

    void Awake()
    {
        MapPartPrefab = (GameObject)Resources.Load("Prefabs/Map/MapPart", typeof(GameObject));
    }

    void Start()
    {
        if (_loadedParts.Count == 0)
        {
            LoadPart(0, 0);
            //StartCoroutine(LoadPartIE(0, 0));
        }
    }

    void FixedUpdate()
    {
        
        if (_loadedParts.Count > 0)
        {
            AroundCheck();
        }
    }


    //checking all closest cells around current map cell to load map parts
    public void AroundCheck()
    {
        for (int y = Y-1; y < Y+2; y++)
        {
            for (int x = X-1; x < X+2; x++)
            {
                if (!_loadedParts.ContainsKey($"{x},{y}")) //looking for a non loaded map parts by (x,y). around the current cell
                {
                    LoadPart(x, y);
                    //StartCoroutine(LoadPartIE(x, y));
                }
            }
        }
    }

    private void LoadPart(int x, int y)
    {
        GameObject MapPart = Instantiate(MapPartPrefab);
        MapPart.GetComponent<MapPart>().MapSize = MapSize;
        MapPart.GetComponent<MapPart>().SetIndex(x, y);
        _loadedParts.Add($"{x},{y}", MapPart);
    }

    IEnumerator LoadPartIE(int x, int y)
    {
        yield return new WaitUntil(() => GPS.instance.isInit);

        print("Start load part " + x + " " + y);

        GameObject MapPart = Instantiate(MapPartPrefab);
        MapPart.GetComponent<MapPart>().MapSize = MapSize;
        MapPart.GetComponent<MapPart>().SetIndex(x, y);
        _loadedParts.Add($"{x},{y}", MapPart);

        print("End load part");
    }
    
    public void SetIndex(Vector2 index)
    {
        X = (int)index.x;
        Y = (int)index.y;
    }
}
