using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CraftableItem : Item
{
    public int BlueprintsAmountToCraft;
    public Dictionary<string, int> materials = new Dictionary<string, int>();
    public bool IsCraftable { set; get; }
    public CraftableItem(CatalogItem item) : base(item)
    {
        var customData = GetCustomData();

        BlueprintsAmountToCraft = int.Parse(customData.BlueprintsAmount);
        var materialsSet = customData.materials.Split(',');
        foreach (var material in materialsSet)
        {
            var set = material.Split(':');
            materials.Add(set[0], int.Parse(set[1]));
        }
    }
}
