using PlayFab;
using PlayFab.ServerModels;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabInventoryService
{

    public delegate void OnGetItem();
    public static OnGetItem onGetItemCallback;

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

        Debug.Log("GrantItemSuccess");
        //inventory update
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
    }
    private static void OnConsumeItemError(PlayFabError error)
    {
        Debug.Log("Consume unsuccessful: " + error.ErrorMessage);
    }
}
