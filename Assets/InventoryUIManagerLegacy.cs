using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManagerLegacy : MonoBehaviour
{
    public static InventoryUIManagerLegacy instance;
    public static Dictionary<ItemTypeLegacy, UIInventoryUnit> items = new Dictionary<ItemTypeLegacy, UIInventoryUnit>();

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

    private void Start()
    {
        //PlayFabInventoryService.onGetInventoryCallback += LoadInventory;
        LoadInventory();
    }

    public void LoadInventory()
    {
        print("LoadInv");
        //foreach (var item in PlayFabInventoryService.items.Values)
        //{
        //    AddToInventory(item);
        //}
    }

    private void AddToInventory(ItemInstance itemInstance)
    {
        var item = ItemManagerLegacy.itemPrefabs.Find(x => x.GetComponent<ItemLegacy>().id.ToString().Equals(itemInstance.DisplayName))?.GetComponent<ItemLegacy>();
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
