using PlayFab.ClientModels;
using System;

[Serializable]
public abstract class ItemData
{
    public string id;
    public string name;
    public string description;
    public string type;
    public int cost;

    public ItemData(CatalogItem item)
    {
        id = item.ItemId;
        name = item.DisplayName;
        description = item.Description;
        type = item.Tags[0];
        cost = (int)item.VirtualCurrencyPrices[TitleInfo.Currency];
    }
}
