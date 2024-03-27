using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour, IPrepare
{
    private Dictionary<string, GameObject> itemPrefabs;

    private Dictionary<string, CollectibleItemData> collectible;
    private Dictionary<string, CraftableItemData> craftable;
    private Dictionary<string, MinableItemData> minable;

    private void ReadPrefabs()
    {
        itemPrefabs = new Dictionary<string, GameObject>();

        var prefabs = Resources.LoadAll<GameObject>("ItemPrefabs/new prefabs").ToList();
        foreach (var prefab in prefabs)
        {
            itemPrefabs.Add(prefab.name, prefab);
        }
    }

    private void InitializeCatalogs()
    {
        collectible = new Dictionary<string, CollectibleItemData>();
        craftable = new Dictionary<string, CraftableItemData>();
        minable = new Dictionary<string, MinableItemData>();
    }

    private void LoadItems(Action<bool, string> onComplete)
    {
        InitializeCatalogs();

        LoadCatalog(TitleInfo.CollectibleCatalogVersion, collectible, item => new CollectibleItemData(item), onComplete);
        LoadCatalog(TitleInfo.CraftableCatalogVersion, craftable, item => new CraftableItemData(item), onComplete);
        LoadCatalog(TitleInfo.MinableCatalogVersion, minable, item => new MinableItemData(item), onComplete);

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
}
