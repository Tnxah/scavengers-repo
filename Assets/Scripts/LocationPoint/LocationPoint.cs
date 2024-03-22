using System;
using UnityEngine;

public class LocationPoint : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected string id;
    [SerializeField]
    protected string pointName;
    [SerializeField]
    protected float latitude;
    [SerializeField]
    protected float longitude;
    [SerializeField]
    protected LocationPointType type;
    [SerializeField]
    protected string description;

    protected Vector3 gameCoordinates;

    public virtual void Interact()
    {
    }

    public void SetData(ILocationPointData data)
    {
        id = data.Id;
        pointName = data.Name;
        latitude = data.Latitude;
        longitude = data.Longitude;
        type = (LocationPointType)Enum.Parse(typeof(LocationPointType), data.Type, true);
        description = data.Description;

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
