using PlayFab.ClientModels;
using System;
using UnityEngine;

[Serializable]
public abstract class Item
{
    public string id;
    public string name;
    public string description;
    public ItemType type;
    public int cost;

    private string customDataJson;

    public Item(CatalogItem item)
    {
        id = item.ItemId;
        name = item.DisplayName;
        description = item.Description;
        type = (ItemType)Enum.Parse(typeof(ItemType), item.Tags[0], true);
        cost = (int)item.VirtualCurrencyPrices[TitleInfo.Currency];

        customDataJson = item.CustomData;
    }

    protected ItemCustomData GetCustomData()
    {
        return JsonUtility.FromJson<ItemCustomData>(customDataJson);
    }
}
