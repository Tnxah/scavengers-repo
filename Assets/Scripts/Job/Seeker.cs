using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Job
{
    private float[] spawnChanceMultiplyer = {1f, 1.1f, 1.2f, 1.3f};
    private float[] detectionRadiusMultiplyer = {1f, 1.15f, 1.25f, 1.35f};
    private float[] spawnDelayMultiplyer = {1f, 0.9f, 0.8f, 0.7f };

    private int[] itemsToLevelUp = {0, 100, 500, 600, 1200};

    private int itemsCollected;

    public Seeker()
    {
        type = JobType.Seeker;
        level = PlayfabStatisticsManager.GetStat(StatisticsKeys.seekerLevelKey);
        unlocked = Convert.ToBoolean(PlayfabStatisticsManager.GetStat(StatisticsKeys.seekerUnlockedKey));
        itemsCollected = PlayfabStatisticsManager.GetStat(StatisticsKeys.itemsCollectedKey);

        Unlock();
    }

    public override void ApplyJobProperties()
    {
        PlayFabInventoryService.onGetItemCallback += OnItemCollected;
        
        PlayerStatistics.currentSpawnChance = PlayerStatistics.baseSpawnChance * spawnChanceMultiplyer[level];

        PlayerStatistics.currentDetectionRadius = PlayerStatistics.baseDetectionRadius * detectionRadiusMultiplyer[level];

        PlayerStatistics.currentSpawnDelay = PlayerStatistics.baseSpawnDelay * spawnDelayMultiplyer[level];

        PlayerScript.player.UpdateBody();
        ItemGenerator.instance.Init();
        Debug.Log(level);
    }

    public override void CancelJobProperties()
    {
        PlayFabInventoryService.onGetItemCallback -= OnItemCollected;

        PlayerStatistics.currentSpawnChance = PlayerStatistics.baseSpawnChance;

        PlayerStatistics.currentDetectionRadius = PlayerStatistics.baseDetectionRadius;

        PlayerStatistics.currentSpawnDelay = PlayerStatistics.baseSpawnDelay;

        PlayerScript.player.UpdateBody();
        ItemGenerator.instance.Init();
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

        if (level < itemsToLevelUp.Length && itemsCollected >= itemsToLevelUp[level + 1])
        {
            LevelUp();  
        }
    }
}
