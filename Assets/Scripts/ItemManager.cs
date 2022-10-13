using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager
{
    public static List<GameObject> itemPrefabs = new List<GameObject>();
    
    public static string catalogVersion;
    public static string currency;


    public static void ReadPrefabs()
    {
        itemPrefabs = Resources.LoadAll<GameObject>("ItemPrefabs").ToList();
    }

    public static void LoadItems()
    {
        catalogVersion = TitleInfo.CatalogVersion;
        currency = TitleInfo.Currency;


        GetCatalogItemsRequest request = new GetCatalogItemsRequest();
        request.CatalogVersion = catalogVersion;

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

            },
            error => {
                Debug.Log(error.ErrorMessage);
            });
    }



    public static Item GetItemPrefab(string id)
    {
        var itemprefab = itemPrefabs.Find(x => x.GetComponent<Item>().id.ToString().Equals(id))?.GetComponent<Item>();
        return itemprefab;
    }

}
