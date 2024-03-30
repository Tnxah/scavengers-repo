using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItem : MonoBehaviour, IInteractable, IHidable
{
    [SerializeField]
    public string itemId;
    public int spawnChance  { get; private set;   }

    private Renderer[] renderers;
    private Floater floater;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        floater = GetComponent<Floater>();

#if !UNITY_EDITOR
#endif
        Hide();
    }

    public virtual void Initialize(CollectibleItem item)
    {
        itemId = item.id;
        spawnChance = item.spawnChance;
    }

    public bool CanInteract() => true;

    public virtual void Interact()
    {
        PlayFabInventoryService.GetItem(itemId);
        Destroy(gameObject);
    }

    public void Hide()
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }

        floater.enabled = false;
    }

    public void Unhide()
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
        }

        floater.enabled = true;
    }
}
