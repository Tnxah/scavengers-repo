using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = System.Random;


public class Item : MonoBehaviour, IInteractable, IHidable
{
    [SerializeField]
    private ItemType id;
    [SerializeField]
    private List<MeshRenderer> modelMesh = new List<MeshRenderer>();
    [SerializeField]
    private List<GameObject> modelPrefabs;
    private Floater floater;
    private bool interactable;

    private Random rnd = new Random();

    // Start is called before the first frame update
    private void Awake()
    {
        RandomiseModel();
        floater = GetComponent<Floater>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact()
    {
        if (!interactable || !PlayerScript.player.IsCloseEnough(transform))
            return;

        PlayFabInventoryService.GetItem(id.ToString());
        Destroy(gameObject);
    }

    private void RandomiseModel()
    {
        var rand = rnd.Next(modelPrefabs.Count);
        var tempModel = modelPrefabs[rand];
        modelMesh = Instantiate(tempModel, transform).GetComponentsInChildren<MeshRenderer>().ToList();

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
}
