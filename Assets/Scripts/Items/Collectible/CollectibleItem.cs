using PlayFab.ClientModels;
using System;

[Serializable]
public class CollectibleItem : Item
{
    public int spawnChance;
    public CollectibleItem(CatalogItem item) : base(item)
    {
        var customData = GetCustomData();

        spawnChance = int.Parse(customData.SpawnChance);
    }
}
