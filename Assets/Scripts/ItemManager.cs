using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ItemManager : IPrepare
{
    public static List<GameObject> itemPrefabs = new List<GameObject>();
    
    public static string catalogVersion;
    public static string currency;

    private static bool ready;

    private static void ReadPrefabs()
    {
        itemPrefabs = Resources.LoadAll<GameObject>("ItemPrefabs").ToList();
    }

    private static void LoadItems(Action onComplete, Action<string> onError)
    {
        catalogVersion = TitleInfo.CatalogVersion;
        currency = TitleInfo.Currency;

        GetCatalogItemsRequest request = new GetCatalogItemsRequest() 
        {
            CatalogVersion = catalogVersion
        };

        PlayFabClientAPI.GetCatalogItems(request,
            result => {
                List<CatalogItem> items = result.Catalog;              
                foreach (var item in items)
                {
                    var prefab = GetItemPrefab(item.ItemId);
                    if (!prefab)
                    {
                        DebugService.Log($"{item.ItemId} has no prefab");
                        continue;
                    }
                    prefab.cost = (int)item.VirtualCurrencyPrices[currency];
                    prefab.description = item.Description;
                }

                ready = true;
                onComplete?.Invoke();
            },
            error => {
                onError?.Invoke(error.ErrorMessage);
            });
    }

    public static ItemLegacy GetItemPrefab(string id)
    {
        var itemprefab = itemPrefabs.Find(x => x.GetComponent<ItemLegacy>().id.ToString().Equals(id))?.GetComponent<ItemLegacy>();
        return itemprefab;
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        try
        {
            ReadPrefabs();

            LoadItems(
                () => {
            // If successful
            Debug.Log($"Prepare ItemManager result: {ready} {null}");
                    onComplete?.Invoke(ready, null);
                },
                (errorMessage) => {
                    // On error
                    Debug.Log($"Prepare ItemManager result: {false} {errorMessage}");
                    onComplete?.Invoke(false, errorMessage);
                }
            );
        }
        catch (Exception ex)
        {
            // On error
            Debug.Log($"Prepare ItemManager result: {false} {ex.Message}");
            onComplete?.Invoke(false, ex.Message);
        }

        yield break;
    }
}
