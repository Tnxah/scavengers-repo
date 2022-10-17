using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ItemGenerator : MonoBehaviour
{
    public static ItemGenerator instance;

    private Random rnd = new Random();
    public int spawnDelay = 30;
    public float spawnChance;
    public float spawnRadius;

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
        //spawnDelay = PlayerStatistics.spawnChance;
        spawnChance = PlayerStatistics.currentSpawnChance;
        spawnRadius = PlayerStatistics.spawnRadius;

        ready = true;
    }


    IEnumerator SlowUpdate()
    {
        var items = ItemManager.itemPrefabs;

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

    public void Prepare()
    {
        Init();
        StartCoroutine(SlowUpdate());
    }

    public bool isReady()
    {
        return ready;
    }
}
