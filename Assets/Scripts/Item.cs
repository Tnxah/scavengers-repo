using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour, IInteractable, IHidable
{
    [SerializeField]
    private ItemType id;
    [SerializeField]
    private MeshRenderer model;
    private Floater floater;
    private bool interactable; 


    // Start is called before the first frame update
    private void Awake()
    {
        floater = GetComponent<Floater>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact()
    {
        if (!interactable && !PlayerScript.player.IsCloseEnough(transform))
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
        GetComponent<SphereCollider>().enabled = false;
    }
}
