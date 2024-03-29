using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItemMoney : SceneItem
{
    private int cost;

    public override void Initialize(CollectibleItem item)
    {
        base.Initialize(item);
        cost = Random.Range(1, item.cost);
    }

    public override void Interact()
    {
        print(cost);
        PlayFabEconomy.IncreaseMoney(cost);
        Destroy(gameObject);
    }
}
