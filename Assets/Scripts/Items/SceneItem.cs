using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItem : MonoBehaviour, IInteractable, IHidable
{
    public string itemId { get; private set; }
    public int spawnChance  { get; private set;   }

    public void Initialize(CollectibleItem item)
    {
        itemId = item.id;
        spawnChance = item.spawnChance;
    }

    public bool CanInteract() => true;

    public void Interact()
    {
        PlayFabInventoryService.GetItem(itemId);
        Destroy(gameObject);
    }

    public void Hide()
    {
        GetComponentsInChildren<RenderBuffer>();
    }

    public void Unhide()
    {
        throw new System.NotImplementedException();
    }
}
