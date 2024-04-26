using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchUIManager : MonoBehaviour
{
    public static WorkbenchUIManager instance;

    private Dictionary<string, UICraftUnit> items = new Dictionary<string, UICraftUnit>();

    public GameObject workbenchPanel;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject craftUiUnitPrefab;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void RefreshItemUnit(CraftableItem item)
    {
        if (items.ContainsKey(item.id))
        {
            items[item.id].SetInteractable(item.IsCraftable);
            return;
        }

        var newItem = Instantiate(craftUiUnitPrefab, content).GetComponent<UICraftUnit>();

        newItem.materials.text = "";
        newItem.name.text = item.id.ToString();
        foreach (var material in item.materials)
        {
            newItem.materials.text += material.Key + ": " + material.Value + " ";
        }

        newItem.materials.text += "blueprints: " + item.BlueprintsAmountToCraft;
        items.Add(item.id, newItem);
        items[item.id].SetInteractable(item.IsCraftable);
    }

    public void OpenCloseInventory()
    {
        if (!workbenchPanel.activeSelf) RefreshList();
        workbenchPanel.SetActive(!workbenchPanel.activeSelf);
    }

    public void RefreshList()
    {
        var availableCrafts = WorkbenchService.GetAvailableCrafts();

        foreach (var craft in availableCrafts)
        {
            RefreshItemUnit(craft);
        }
    }
}
