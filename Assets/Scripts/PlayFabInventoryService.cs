using PlayFab;
using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using System.Linq;

public class PlayFabInventoryService
{
    public static event Action<ItemInstance> onItemAmountChangedCallback;

    private static Dictionary<string, ItemInstance> items = Inventory.items;
    public static void GrantItem(List<string> items)
    {
        PlayFabServerAPI.GrantItemsToUser(new PlayFab.ServerModels.GrantItemsToUserRequest
        {
            CatalogVersion = TitleInfo.CatalogVersion,
            PlayFabId = AccountManager.playerId,
            ItemIds = items

        }, OnGrantSuccess, OnError);
    }

    public static void GrantItem(string item, string catalogVersion)
    {
        var items = new List<string> { item };

        PlayFabServerAPI.GrantItemsToUser(new PlayFab.ServerModels.GrantItemsToUserRequest
        {
            CatalogVersion = catalogVersion,
            PlayFabId = AccountManager.playerId,
            ItemIds = items

        }, OnGrantSuccess, OnError);
    }

    private static void OnGrantSuccess(PlayFab.ServerModels.GrantItemsToUserResult result)
    {
        ItemInstance itemInstance;

        foreach (var grantedItem in result.ItemGrantResults)
        {
            if (items.ContainsKey(grantedItem.ItemId))
            {
                itemInstance = items[grantedItem.ItemId];

                itemInstance.RemainingUses = grantedItem.RemainingUses;
            
                Debug.Log($"Item granted successfully. Local inventory updated. Item: {itemInstance.RemainingUses} in dictionary: {items[itemInstance.ItemId].RemainingUses}");
            }
            else
            {
                itemInstance = new ItemInstance
                {
                    ItemId = grantedItem.ItemId,
                    ItemInstanceId = grantedItem.ItemInstanceId,
                    RemainingUses = grantedItem.RemainingUses
                };

                items.Add(itemInstance.ItemId, itemInstance);
            }

            onItemAmountChangedCallback?.Invoke(itemInstance);
        }
    }

    public static void ConsumeItem(string itemId, int count = 1)
    {
        if (!items.ContainsKey(itemId) || items[itemId].RemainingUses < count) return;

        ConsumeItemRequest request = new ConsumeItemRequest();
        request.ItemInstanceId = items[itemId].ItemInstanceId;
        request.ConsumeCount = count;

        PlayFabClientAPI.ConsumeItem(request, OnConsumeItemSuccess, OnError);
    }

    private static void OnConsumeItemSuccess(ConsumeItemResult result)
    {
        var item = items.Values.ToList().FirstOrDefault(item => item.ItemInstanceId == result.ItemInstanceId);
        if (item != null)
        {
            item.RemainingUses = result.RemainingUses;
            if (item.RemainingUses <= 0)
            {
                items.Remove(item.ItemId);
            }
            //Debug.Log($"Item consumed successfully. Local inventory updated. item: {item.RemainingUses} in dictionary: {items[item.ItemId].RemainingUses}");

            onItemAmountChangedCallback?.Invoke(item);
        }
    }

    public static void GetUserInventory(Action onComplete)
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result => 
        {
            foreach (var item in result.Inventory)
            {
                if (items.ContainsKey(item.ItemId))
                {
                    items[item.ItemId].RemainingUses = item.RemainingUses;
                    continue;
                }

                items.Add(item.ItemId, item);

                onComplete?.Invoke();
            }
        },
        OnError);
    }

    private static void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        foreach (var item in result.Inventory)
        {
            if (items.ContainsKey(item.ItemId))
            {
                items[item.ItemId].RemainingUses = item.RemainingUses;
                continue;
            }

            items.Add(item.ItemId, item);
        }
    }

    private static void OnError(PlayFabError error)
    {
        Debug.LogError("PlayFab Inventory error: " + error.ErrorMessage);
    }
}
