using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoint : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected ResourcePointType type;
    [SerializeField]
    private string neededToolId;
    [SerializeField]
    private string minableResourceId;

    protected Vector3 gameCoordinates;

    public bool CanInteract()
    {
        return neededToolId.Equals("") ? true : InventoryUIManager.instance.HasTool(neededToolId);
    }

    public virtual void Interact()
    {
        PlayFabInventoryService.GetItem(minableResourceId);
        PlayFabInventoryService.ConsumeItem(neededToolId);
    }

    public void SetData(Vector2Int cell, string type)
    {
        this.type = (ResourcePointType)Enum.Parse(typeof(ResourcePointType), type, true);

        var GPScoordinates = GridManager.GridToGPSCenter(cell);
        gameCoordinates = CoordinateConverter.GPSToGamePosition(GPScoordinates.latitude, GPScoordinates.longitude);
        transform.position = gameCoordinates;
    }
}
