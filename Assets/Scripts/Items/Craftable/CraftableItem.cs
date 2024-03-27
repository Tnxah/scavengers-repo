using PlayFab.ClientModels;
using System;

[Serializable]
public class CraftableItem : Item
{
    public string BlueprintName;
    public int BlueprintsAmountToCraft;

    public CraftableItem(CatalogItem item) : base(item)
    {
        var customData = GetCustomData();

        BlueprintName = customData.BlueprintName;
        BlueprintsAmountToCraft = int.Parse(customData.BlueprintsAmount);
    }
}
