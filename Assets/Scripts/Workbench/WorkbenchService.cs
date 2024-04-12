using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchService : MonoBehaviour
{
    public static List<CraftableItem> GetAvailableCrafts()
    {
        List<CraftableItem> availableCrafts = new List<CraftableItem>();
        List<string> craftableKeys = ItemManager.GetCraftableKeys();

        foreach (var key in craftableKeys)
        {
            CraftableItem item = ItemManager.TryGetCraftable(key);
            int ownedBlueprints = BlueprintService.GetAmountOfBlueprints(key);
            bool hasEnoughBlueprints = ownedBlueprints >= item.BlueprintsAmountToCraft;
            bool hasEnoughMaterials = true;

            foreach (var material in item.materials)
            {
                if (!Inventory.HasItem(material.Key, material.Value))
                {
                    hasEnoughMaterials = false;
                    break;
                }
            }

            item.IsCraftable = hasEnoughBlueprints && hasEnoughMaterials;
            availableCrafts.Add(item);
        }

        return availableCrafts;
    }

    public static void CraftItem(string itemId)
    {
        var craftableItem = ItemManager.TryGetCraftable(itemId);

        foreach (var material in craftableItem.materials)
        {
            PlayFabInventoryService.ConsumeItem(material.Key, material.Value);
        }

        PlayFabInventoryService.GrantItem(itemId, TitleInfo.CraftableCatalogVersion);
    }
}
