using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour, IPrepare
{
    public static InventoryUIManager instance;

    private Dictionary<string, UIInventoryUnit> resources;
    private Dictionary<string, UIInventoryUnit> tools;

    public GameObject inventoryPanel;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject inventoryUiUnitPrefab;

    public DeleteConfirmation deleteConfirmation;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        resources = new Dictionary<string, UIInventoryUnit>();
        tools = new Dictionary<string, UIInventoryUnit>();
    }

    public void LoadInventory()
    {
        print("LoadInv");
        foreach (var item in PlayFabInventoryService.items)
        {
            print(item.ItemId);
            AddToInventory(item);
        }
    }

    private void AddToInventory(ItemInstance itemInstance)
    {
        Item item;

        if ((item = ItemManager.TryGetCollectible(itemInstance.ItemId)) != null || (item = ItemManager.TryGetMinable(itemInstance.ItemId)) != null)
            AddTo(resources, item, itemInstance);
        else if ((item = ItemManager.TryGetCraftable(itemInstance.ItemId)) != null && item.type.Equals(ItemType.TOOL))
        {
            print(item.id);
            AddTo(tools, item, itemInstance);
        }
    }

    private void AddTo(Dictionary<string, UIInventoryUnit> catalog, Item item, ItemInstance itemInstance)
    {
        if (catalog.ContainsKey(item.id))
        {
            catalog[item.id].count.text = itemInstance.RemainingUses.ToString();
            return;
        }

        var newItem = Instantiate(inventoryUiUnitPrefab, content).GetComponent<UIInventoryUnit>();

        newItem.name.text = item.id.ToString();
        newItem.description.text = item.description;
        newItem.id = itemInstance.ItemInstanceId;
        newItem.count.text = itemInstance.RemainingUses.ToString();
        //newItem.icon = item.icon;

        catalog.Add(item.id, newItem);
    }

    public bool HasTool(string toolId)
    {
        return tools.ContainsKey(toolId);
    }

    public void OpenCloseInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {

        PlayFabInventoryService.onGetInventoryCallback += LoadInventory;
        LoadInventory();

        onComplete?.Invoke(true, null);

        yield break;
    }
}
