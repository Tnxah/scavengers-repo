using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager instance;
    public static Dictionary<ItemType, UIInventoryUnit> items = new Dictionary<ItemType, UIInventoryUnit>();

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
        PlayFabInventoryService.onGetInventoryCallback += LoadInventory;
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
        var item = ItemManager.itemPrefabs.Find(x => x.GetComponent<Item>().id.ToString().Equals(itemInstance.DisplayName))?.GetComponent<Item>();
        if (!item)
            return;

        if (items.ContainsKey(item.id))
        {
            items[item.id].count.text = itemInstance.RemainingUses.ToString();
            return;
        }

        var newItem = Instantiate(inventoryUiUnitPrefab, content).GetComponent<UIInventoryUnit>();

        newItem.name.text = item.id.ToString();
        newItem.description.text = item.description;
        newItem.id = itemInstance.ItemInstanceId;
        newItem.count.text = itemInstance.RemainingUses.ToString();
        newItem.icon = item.icon;

        items.Add(item.id, newItem);
    }

    public void OpenCloseInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}
