using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : ItemLegacy
{
    private int  max = 15;

    public override void Awake()
    {
        base.Awake();
        cost += Random.Range(cost - 1, max);
    }

    public override void Interact()
    {
        if (!interactable)
            return;

        PlayFabEconomy.IncreaseMoney(cost);
        Destroy(gameObject);
    }
}
