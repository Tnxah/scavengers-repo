using PlayFab;
using PlayFab.ServerModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabInventoryService
{
    public static event Action onGetItemCallback;

    public static event Action onGetInventoryCallback;

    public static event Action onConsumeItemCallback;

    public static bool getInventoryReady;

    public static List<PlayFab.ClientModels.ItemInstance> items = new List<PlayFab.ClientModels.ItemInstance>();

    public static void GetItem(List<string> items)
    {
        PlayFabServerAPI.GrantItemsToUser(new GrantItemsToUserRequest
        {
            CatalogVersion = TitleInfo.CatalogVersion,
            PlayFabId = AccountManager.playerId,
            ItemIds = items

        }, OnGetSuccess, OnGetError);
    }


    public static void GetItem(string item)
    {
        var items = new List<string> { item };

        PlayFabServerAPI.GrantItemsToUser(new GrantItemsToUserRequest
        {
            CatalogVersion = TitleInfo.CatalogVersion,
            PlayFabId = AccountManager.playerId,
            ItemIds = items

        }, OnGetSuccess, OnGetError);
    }


    private static void OnGetError(PlayFabError error)
    {
        Debug.Log("GrantItemError: " + error.ErrorMessage);
    }


    private static void OnGetSuccess(GrantItemsToUserResult result)
    {
        onGetItemCallback?.Invoke();

        GetUserInventory();
        Debug.Log("GrantItemSuccess");
    }


    public static void ConsumeItem(string itemId, int count = 1)
    {
        PlayFab.ClientModels.ConsumeItemRequest request = new PlayFab.ClientModels.ConsumeItemRequest();
        request.ItemInstanceId = itemId;
        request.ConsumeCount = count;

        PlayFabClientAPI.ConsumeItem(request, OnConsumeItemSuccess, OnConsumeItemError);
    }


    private static void OnConsumeItemSuccess(PlayFab.ClientModels.ConsumeItemResult result)
    {
        Debug.Log("Consume Successful");
        GetUserInventory();

        onConsumeItemCallback?.Invoke();
    }


    private static void OnConsumeItemError(PlayFabError error)
    {
        Debug.Log("Consume unsuccessful: " + error.ErrorMessage);
    }


    public static void GetUserInventory()
    {
        PlayFabClientAPI.GetUserInventory(new PlayFab.ClientModels.GetUserInventoryRequest(), OnGetUserInventorySuccess, OnGetUserInventoryError);
    }


    private static void OnGetUserInventorySuccess(PlayFab.ClientModels.GetUserInventoryResult result)
    {
        Debug.Log("Get inventory Successful");

        items = result.Inventory;

        onGetInventoryCallback?.Invoke();

        getInventoryReady = true;
    }


    private static void OnGetUserInventoryError(PlayFabError error)
    {
        Debug.Log("Get inventory unsuccessful: " + error.ErrorMessage);
    }
}
