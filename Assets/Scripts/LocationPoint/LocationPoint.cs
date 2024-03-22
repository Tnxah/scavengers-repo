using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationPoint : MonoBehaviour, IInteractable, IHidable
{
    [SerializeField]
    public string id;
    [SerializeField]
    private string pointName;
    [SerializeField]
    private float latitude;
    [SerializeField]
    private float longitude;
    [SerializeField]
    private LocationPointType type;
    [SerializeField]
    private string description;


    private Vector3 gameCoordinates;

    public void Interact()
    {
        if (PlayerScript.player.IsCloseEnough(transform))
            DebugService.Log($"id:{id}\nname: {pointName}\nlat/lon: {latitude}/{longitude}\ntype: {type}\ndescription: {description}");
    }

    public void SetData(LocationPointData data)
    {
        id = data.id;
        pointName = data.name;
        latitude = data.latitude;
        longitude = data.longitude;
        type = (LocationPointType)Enum.Parse(typeof(LocationPointType), data.type, true);
        description = data.description;

        gameCoordinates = CoordinateConverter.GPSToGamePosition(latitude, longitude);
        transform.position = gameCoordinates;
    }

    public float DistanceToPlayer()
    {
        return Vector3.Distance(PlayerScript.player.transform.position, gameCoordinates);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Unhide()
    {
        gameObject.SetActive(true);
    }
}
