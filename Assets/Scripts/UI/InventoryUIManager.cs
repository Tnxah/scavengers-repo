using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour, IPrepare
{
    public static InventoryUIManager instance;

    private Dictionary<string, UIInventoryUnit> items = new Dictionary<string, UIInventoryUnit>();

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
    }

    private void RefreshItemUnit(ItemInstance itemInstance)
    {
        if (itemInstance.RemainingUses == 0)
        {
            Destroy(items[itemInstance.ItemId].gameObject);
            items.Remove(itemInstance.ItemId);
            return;
        }

        if (items.ContainsKey(itemInstance.ItemId))
        {
            items[itemInstance.ItemId].count.text = ((int)itemInstance.RemainingUses).ToString();
            return;
        }

        var newItem = Instantiate(inventoryUiUnitPrefab, content).GetComponent<UIInventoryUnit>();
        Item item = ItemManager.TryGetCollectible(itemInstance.ItemId);
        if (item == null) item = ItemManager.TryGetCraftable(itemInstance.ItemId);
        if (item == null) item = ItemManager.TryGetMinable(itemInstance.ItemId);

        newItem.name.text = item.id.ToString();
        newItem.description.text = item.description;
        newItem.count.text = ((int)itemInstance.RemainingUses).ToString();

        items.Add(item.id, newItem);
    }

    public void OpenCloseInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        PlayFabInventoryService.onItemAmountChangedCallback += RefreshItemUnit;

        var isComplete = false;
        PlayFabInventoryService.GetUserInventory(() => { isComplete = true; });
        yield return new WaitUntil(() => isComplete);

        foreach (var item in Inventory.items)
        {
            RefreshItemUnit(item.Value);
        }

        onComplete?.Invoke(true, null);
        yield break;
    }

    private void OnDisable()
    {
        PlayFabInventoryService.onItemAmountChangedCallback -= RefreshItemUnit;
    }
}
