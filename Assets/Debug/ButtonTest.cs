using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    public GameObject mapPrefab;
    public void Refresh()
    {
        InventoryUIManagerLegacy.instance.LoadInventory();
    }

    public void SpawnMap()
    {
        Instantiate(mapPrefab);
    }
}
