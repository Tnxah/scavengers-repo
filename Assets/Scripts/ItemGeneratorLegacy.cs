using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ItemGeneratorLegacy : MonoBehaviour, IPrepare
{
    public static ItemGeneratorLegacy instance;

    private Random rnd = new Random();
    private float spawnDelay;
    private float spawnChance;
    private float spawnRadius;

    private bool ready;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public void Init()
    {
        spawnDelay = PlayerStatistics.currentSpawnDelay;
        spawnChance = PlayerStatistics.currentSpawnChanceMultipier;
        spawnRadius = PlayerStatistics.spawnRadius;

        ready = true;
    }


    IEnumerator SlowUpdate()
    {
        var items = ItemManagerLegacy.itemPrefabs;

        while (true) { 
            var rand = rnd.Next(0, 100);
            print(rand);
            if (rand < spawnChance)
            {
                    var position = UnityEngine.Random.insideUnitCircle * (spawnRadius);
                    Vector3 pos = PlayerScript.player.transform.position + new Vector3(position.x, 0, position.y);
                    Instantiate(items[new Random().Next(items.Count)], pos, Quaternion.identity);
            }


            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        try
        {
            Init();
            StartCoroutine(SlowUpdate());

            // If successful
            Console.WriteLine($"Prepare ItemGenerator result: {ready} {null}");
            onComplete?.Invoke(ready, null);
        }
        catch (Exception ex)
        {
            // On error
            Console.WriteLine($"Prepare ItemGenerator result: {false} {ex.Message}");
            onComplete?.Invoke(false, ex.Message);
        }

        yield break;
    }
}
