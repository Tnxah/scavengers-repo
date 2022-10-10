using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable, IHidable
{
    [SerializeField]
    private ItemType id;
    [SerializeField]
    private MeshRenderer model;
    [SerializeField]
    private Floater floater;
    private bool interactable = false; 


    // Start is called before the first frame update
    private void Awake()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact()
    {
        if (!interactable)
            return;

        PlayFabInventoryService.GetItem(id.ToString());
        Destroy(gameObject);
    }

    public void Hide()
    {
        interactable = false;
        model.enabled = false;
        floater.enabled = false;
    }

    public void Unhide()
    {
        interactable = true;
        model.enabled = true;
        floater.enabled = true;
    }
}
