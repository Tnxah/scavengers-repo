using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoint : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected ResourcePointType type;

    protected Vector3 gameCoordinates;

    public bool CanInteract()
    {
        throw new NotImplementedException();
    }

    public virtual void Interact()
    {
    }

    public void SetData(Vector2Int cell, string type)
    {
        this.type = (ResourcePointType)Enum.Parse(typeof(ResourcePointType), type, true);

        var GPScoordinates = GridManager.GridToGPSCenter(cell);
        gameCoordinates = CoordinateConverter.GPSToGamePosition(GPScoordinates.latitude, GPScoordinates.longitude);
        transform.position = gameCoordinates;
    }
}
