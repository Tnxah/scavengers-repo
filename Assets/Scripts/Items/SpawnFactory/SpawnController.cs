using System;
using System.Collections;
using UnityEngine;

public class SpawnController : MonoBehaviour, IPrepare
{
    public static SpawnController instance;

    private float spawnDelay;
    private float spawnChanceMultiplier;
    private float spawnRadius;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        Initialize();
        StartCoroutine(SpawnTimer());

        onComplete?.Invoke(true, null);
        yield break;
    }

    private void Initialize()
    {
        spawnDelay = PlayerStatistics.currentSpawnDelay;
        spawnChanceMultiplier = PlayerStatistics.currentSpawnChanceMultipier;
        spawnRadius = PlayerStatistics.spawnRadius;
    }

    private IEnumerator SpawnTimer()
    {
        while (true)
        {
            ItemFactory.TrySpawnItem(spawnChanceMultiplier, spawnRadius);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
