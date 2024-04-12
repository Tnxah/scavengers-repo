using System;
using UnityEngine;

public class ResourcePoint : MonoBehaviour, IInteractable
{
    private const string countPostfix = "_count", timePostfix = "_time";
 
    protected string resourcePointId;
    [SerializeField]
    protected ResourcePointType type;
    [SerializeField]
    private string neededToolId;
    [SerializeField]
    private string minableResourceId;

    protected Vector3 gameCoordinates;

    [SerializeField]
    private int interactionsPerDay;

    public virtual bool CanInteract()
    {
        return neededToolId.Equals("") ? true : Inventory.HasTool(neededToolId) && EnoughTimeToInteract();
    }

    private bool EnoughTimeToInteract()
    {
        if (!PlayfabUserDataService.UserData.ContainsKey(resourcePointId + countPostfix)) return true;

        int interactions = PlayfabUserDataService.GetCount(resourcePointId);

        if (PlayFabTimeService.WasYesterday(DateTime.Parse(PlayfabUserDataService.UserData[resourcePointId + timePostfix])))
        {
            interactions = 0;
            PlayfabUserDataService.SetUserData(resourcePointId + countPostfix, interactions.ToString());
            return true;
        }
        else if (interactions < interactionsPerDay)
        {
            return true;
        }

        return false;
    }

    public virtual void Interact()
    {
        PlayFabInventoryService.GrantItem(minableResourceId, TitleInfo.MinableCatalogVersion);
        PlayFabInventoryService.ConsumeItem(neededToolId);

        PlayfabUserDataService.SetUserData(resourcePointId + timePostfix, PlayFabTimeService.CurrentTime().ToString());

        int interactions = PlayfabUserDataService.GetCount(resourcePointId);
        PlayfabUserDataService.SetUserData(resourcePointId + countPostfix, (interactions + 1).ToString());
    }

    public void SetData(Vector2Int cell, string type)
    {
        this.resourcePointId = cell.x + "_" + cell.y;
        this.type = (ResourcePointType)Enum.Parse(typeof(ResourcePointType), type, true);

        var GPScoordinates = GridManager.GridToGPSCenter(cell);
        gameCoordinates = CoordinateConverter.GPSToGamePosition(GPScoordinates.latitude, GPScoordinates.longitude);
        transform.position = gameCoordinates;
    }
}
