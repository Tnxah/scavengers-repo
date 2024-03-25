using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private ResourcePointFactory resourcePointFactory;
    private GridManager gridManager;


    // Start is called before the first frame update
    void Start()
    {
        gridManager = new GridManager(1.5f);
        resourcePointFactory = new ResourcePointFactory(15072001);

        var gridCell = gridManager.GetGridCellFromLocation(GPSController.latitude, GPSController.longitude);
        resourcePointFactory.PlaceFeatureInCell(gridCell);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
