using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItem : MonoBehaviour, IInteractable
{
    //private IItemData data;
    private ICollectible collectible;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {

        //PlayFabInventoryService.GetItem(Id.ToString());
        collectible.Collect();
        Destroy(gameObject);
    }

    public void SetObject(IInteractable interactable){ 
    
    }
}
