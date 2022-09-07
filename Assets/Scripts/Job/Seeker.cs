using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Job
{
    private float[] spawnChanceMultiplyer = {1f, 1.1f, 1.2f, 1.3f};

    private int[] levelUps = {0, 100, 500, 600, 1200};

    private int itemsCollected;

    public Seeker()
    {
        type = JobType.Seeker;
        level = PlayfabStatisticsManager.GetStat(StatisticsKeys.seekerLevelKey);
        unlocked = Convert.ToBoolean(PlayfabStatisticsManager.GetStat(StatisticsKeys.seekerUnlockedKey));
        itemsCollected = PlayfabStatisticsManager.GetStat(StatisticsKeys.itemsCollectedKey);

        //Inventory.instance.onInventoryIncreasedCallback += OnItemCollected;
    }

    public override void ApplyJobProperties()
    {
        //SpawnController.instance.spawnChance = 50 * spawnChanceMultiplyer[level];
        Debug.Log(level);
    }

    public override void LevelUp()
    {
        level++;

        ApplyJobProperties();

        //GetReward();

        PlayfabStatisticsManager.SaveStat(StatisticsKeys.seekerLevelKey, level);
    }

    public override bool Unlock()
    {
        var playerLevel = PlayfabStatisticsManager.GetStat(StatisticsKeys.playerLevelKey);

        if (playerLevel >= 2)
        {
            unlocked = true;
            PlayfabStatisticsManager.SaveStat(StatisticsKeys.seekerUnlockedKey, Convert.ToInt32(unlocked));
        }

        return unlocked;
    }

    public void OnItemCollected()
    {
        Debug.Log("item collected");

        itemsCollected++;

        PlayfabStatisticsManager.SaveStat(StatisticsKeys.itemsCollectedKey, itemsCollected);

        if (level < levelUps.Length && itemsCollected >= levelUps[level + 1])
        {
            LevelUp();  
        }
    }
}
