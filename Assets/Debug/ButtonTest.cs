using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    public GameObject mapPrefab;
    public void Refresh()
    {
        InventoryUIManager.instance.LoadInventory();
    }
    public void AddItem()
    {
        PlayFabInventoryService.ConsumeItem("8B9DA07783057898", 2);
    }

    public void SpawnMap()
    {
        Instantiate(mapPrefab);
    }
}
