using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class ItemFactory
{
    public static void TrySpawnItem(float spawnChanceMultiplier, float spawnRadius)
    {
        var randomKey = ItemManager.GetRandomCollectibleKey();
        var prefab = ItemManager.TryGetItemPrefab(randomKey);
        var collectibleData = ItemManager.TryGetCollectible(randomKey);

        SceneItem sceneItem;

        if (prefab == null || collectibleData == null || !prefab.TryGetComponent(out sceneItem)) return;
        
        sceneItem.Initialize(collectibleData);

        var spawnChance = sceneItem.spawnChance * spawnChanceMultiplier;
        var chance = new Random().Next(100);

        if (chance <= spawnChance)
        {
            var position = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = PlayerScript.player.transform.position + new Vector3(position.x, 0, position.y);
            GameObject.Instantiate(sceneItem.gameObject, spawnPosition, Quaternion.identity);
        }
        
    }
}
