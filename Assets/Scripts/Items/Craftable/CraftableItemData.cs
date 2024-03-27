using PlayFab.ClientModels;
using System;
using UnityEngine;

[Serializable]
public class CraftableItemData : ItemData
{
    public string BlueprintName;
    public int BlueprintsAmountToCraft;

    public CraftableItemData(CatalogItem item) : base(item)
    {
        Debug.Log(item.CustomData);
        BlueprintName = item.CustomData;
    }
}
