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
            AddToInventory(item);
        }
    }

    private void AddToInventory(ItemInstance itemInstance)
    {
        var item = ItemManager.TryGetCollectible(itemInstance.ItemId);
        if (item == null)
            return;

        if (resources.ContainsKey(item.id))
        {
            resources[item.id].count.text = itemInstance.RemainingUses.ToString();
            return;
        }

        var newItem = Instantiate(inventoryUiUnitPrefab, content).GetComponent<UIInventoryUnit>();

        newItem.name.text = item.id.ToString();
        newItem.description.text = item.description;
        newItem.id = itemInstance.ItemInstanceId;
        newItem.count.text = itemInstance.RemainingUses.ToString();
        //newItem.icon = item.icon;

        resources.Add(item.id, newItem);
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
