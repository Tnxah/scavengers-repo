using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class ItemFactory : MonoBehaviour, IPrepare
{
    public static ItemFactory instance;

    private Random rnd = new Random();
    private float spawnDelay;
    private float spawnChanceMultiplier;
    private float spawnRadius;

    public IEnumerator Prepare(System.Action<bool, string> onComplete)
    {
        Initialize();
        StartCoroutine(Timer());

        onComplete?.Invoke(true, null);
        yield break;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Initialize()
    {
        spawnDelay = PlayerStatistics.currentSpawnDelay;
        spawnChanceMultiplier = PlayerStatistics.currentSpawnChanceMultipier;
        spawnRadius = PlayerStatistics.spawnRadius;
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            var sceneItem = TryCreateItem();
            var chance = new Random().Next(100);
            print($"{chance} {sceneItem.spawnChance} {sceneItem.spawnChance * spawnChanceMultiplier}");
            if (sceneItem != null && chance <= sceneItem.spawnChance * spawnChanceMultiplier)
            {
                var position = UnityEngine.Random.insideUnitCircle * (spawnRadius);
                Vector3 pos = PlayerScript.player.transform.position + new Vector3(position.x, 0, position.y);
                Instantiate(sceneItem.gameObject, pos, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private SceneItem TryCreateItem()
    {
        var randomKey = ItemManager.GetRandomCollectibleKey();
        var prefab = ItemManager.TryGetItemPrefab(randomKey);
        var collectibleData = ItemManager.TryGetCollectible(randomKey);
        SceneItem sceneItem;

        if (prefab != null && collectibleData != null && prefab.TryGetComponent(out sceneItem))
        {
            sceneItem.Initialize(collectibleData);
            return sceneItem;
        }
        else
        {
            return null;
        }
    }
}
