using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItemMoney : SceneItem
{
    [SerializeField]
    private int cost;

    public override void Initialize(CollectibleItem item)
    {
        base.Initialize(item);
        cost = Random.Range(1, item.cost + 1);
    }

    public override void Interact()
    {
        PlayFabEconomy.IncreaseMoney(cost);
        Destroy(gameObject);
    }
}
