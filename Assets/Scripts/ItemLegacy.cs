using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = System.Random;
using UnityEngine.UI;

public class ItemLegacy : MonoBehaviour, IInteractable, IHidable
{
    [SerializeField]
    public ItemTypeLegacy id;
    public int cost;
    public string description;

    protected List<MeshRenderer> modelMesh = new List<MeshRenderer>();
    [SerializeField]
    protected List<GameObject> modelPrefabs;
    protected Floater floater;
    protected bool interactable = false;

    public Image icon;

    private Random rnd = new Random();

    // Start is called before the first frame update
    public virtual void Awake()
    {
        RandomiseModel();
        floater = GetComponent<Floater>();
#if UNITY_EDITOR
        Unhide();
#endif
    }

    public virtual void Interact()
    {
        if (!interactable)
            return;

        PlayFabInventoryService.GetItem(id.ToString());
        Destroy(gameObject);
    }

    private void RandomiseModel()
    {
        var rand = rnd.Next(modelPrefabs.Count);
        if (modelPrefabs.Count > 0)
        {
            var tempModel = modelPrefabs[rand];

            modelMesh = Instantiate(tempModel, transform).GetComponentsInChildren<MeshRenderer>().ToList();
        }

    }

    public void Hide()
    {
        interactable = false;
        foreach (var model in modelMesh)
        {
            model.enabled = false;
        }
        floater.enabled = false;
    }

    public void Unhide()
    {
        interactable = true; 
        foreach (var model in modelMesh)
        {
            model.enabled = true;
        }
        floater.enabled = true;
    }

    public bool CanInteract()
    {
        throw new System.NotImplementedException();
    }
}
