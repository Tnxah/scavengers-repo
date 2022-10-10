using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour
{
    private GameObject model;
    SphereCollider sphereCollider;

    private void Awake()
    {
        model = GetComponentInChildren<MeshRenderer>().gameObject;
        sphereCollider = GetComponent<SphereCollider>();
        Hide();
    }
     
    public void Find()
    {
        model.SetActive(true);
        sphereCollider.enabled = true;
    }
    private void Hide()
    {
        model.SetActive(false);
        sphereCollider.enabled = false;
    }
}
