using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : IPrepare
{
    private static Dictionary<string, GameObject> itemsToSpawn;
    private static Dictionary<string, CollectibleItem> collectible;
    private static Dictionary<string, CraftableItem> craftable;
    private static Dictionary<string, MinableItem> minable;

    private void ReadPrefabs()
    {
        itemsToSpawn = new Dictionary<string, GameObject>();

        var prefabs = Resources.LoadAll<GameObject>("ItemPrefabs/new prefabs").ToList();
        foreach (var prefab in prefabs)
        {
            itemsToSpawn.Add(prefab.name, prefab);
        }
    }

    private void InitializeCatalogs()
    {
        collectible = new Dictionary<string, CollectibleItem>();
        craftable = new Dictionary<string, CraftableItem>();
        minable = new Dictionary<string, MinableItem>();
    }

    private IEnumerator LoadItems(Action<bool, string> onComplete)
    {
        InitializeCatalogs();

        bool isCompleted = false;
        bool success = true;
        string errorMsg = null;

        LoadCatalog(TitleInfo.CollectibleCatalogVersion, collectible, item => new CollectibleItem(item), (result, error) =>
        {
            success = result? success : false;
            errorMsg = error;
            isCompleted = true;
        });

        yield return new WaitUntil(() => isCompleted);
        isCompleted = false;


        LoadCatalog(TitleInfo.CraftableCatalogVersion, craftable, item => new CraftableItem(item), (result, error) =>
        {
            success = result ? success : false;
            errorMsg = error;
            isCompleted = true;
        });

        yield return new WaitUntil(() => isCompleted);
        isCompleted = false;
        LoadCatalog(TitleInfo.MinableCatalogVersion, minable, item => new MinableItem(item), (result, error) =>
        {
            success = result ? success : false;
            errorMsg = error;
            isCompleted = true;
        });

        yield return new WaitUntil(() => isCompleted);

        if (!success)
        {
            onComplete?.Invoke(false, "ItemManager catalog not loaded");
            yield break;
        }

        onComplete?.Invoke(true, null);
    }

    private void LoadCatalog<T>(string catalog, Dictionary<string, T> catalogDictionary, Func<CatalogItem, T> createInstance, Action<bool, string> onComplete)
    {
        GetCatalogItemsRequest request = new GetCatalogItemsRequest()
        {
            CatalogVersion = catalog
        };

        PlayFab.PlayFabClientAPI.GetCatalogItems(request,
            result => {
                List<CatalogItem> items = result.Catalog;

                foreach (var item in items)
                {
                    var data = createInstance(item);

                    catalogDictionary.Add(item.ItemId, data);
                }
            },
            error => {
                onComplete?.Invoke(false, error.ErrorMessage);
            });
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        ReadPrefabs();

        LoadItems(onComplete);

        yield break;
    }

    public static GameObject TryGetItemPrefab(string key)
    {
        return itemsToSpawn.ContainsKey(key) ? itemsToSpawn[key] : null;
    }
    public static CollectibleItem TryGetCollectible(string key)
    {
        return collectible.ContainsKey(key) ? collectible[key] : null;
    }
    public static string GetRandomCollectibleKey()
    {
        var rand = new System.Random();
        return collectible.ElementAt(rand.Next(0, collectible.Count - 1)).Key;
    }
}
