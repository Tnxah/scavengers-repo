using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<string, ItemInstance> resources;
    private Dictionary<string, ItemInstance> tools;

    public Inventory()
    {
        AddToInventory(PlayFabInventoryService.GetUserInventory());
    }

    private void AddToInventory(List<ItemInstance> itemInstances)
    {
        foreach (var itemInstance in itemInstances)
        {
            Add(itemInstance);
        }        
    }

    private void Add(ItemInstance itemInstance)
    {
        Dictionary<string, ItemInstance> catalog;

        if ((ItemManager.TryGetCollectible(itemInstance.ItemId)) != null || (ItemManager.TryGetMinable(itemInstance.ItemId)) != null)
        {
            catalog = resources;
        }
        else 
        {
            catalog = tools;
        }

        if (!catalog.ContainsKey(itemInstance.ItemId))
        {
            catalog.Add(itemInstance.ItemId, itemInstance);
        }
    }
}
