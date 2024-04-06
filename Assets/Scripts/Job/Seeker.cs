using System;
using UnityEngine;

public class Seeker : Job
{
    private const int unlockLevel = 2;

    private float[] spawnChanceMultiplier = {1f, 1.2f, 1.4f, 1.6f};
    private float[] detectionRadiusMultiplier = {1f, 1.15f, 1.25f, 1.35f};
    private float[] spawnDelayMultiplier = {1f, 0.9f, 0.8f, 0.7f };

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
        //PlayFabInventoryService.onGrantItemCallback += OnItemCollected; //TODO item collected only for fouded items
        
        PlayerStatistics.currentSpawnChanceMultipier = PlayerStatistics.baseSpawnChanceMultipier * spawnChanceMultiplier[level];

        PlayerStatistics.currentDetectionRadius = PlayerStatistics.baseDetectionRadius * detectionRadiusMultiplier[level];

        PlayerStatistics.currentSpawnDelay = PlayerStatistics.baseSpawnDelay * spawnDelayMultiplier[level];

        PlayerScript.player.UpdateBody();
        ItemGeneratorLegacy.instance.Init();
        Debug.Log(level);
    }

    public override void CancelJobProperties()
    {
        //PlayFabInventoryService.onGrantItemCallback -= OnItemCollected; //TODO item collected only for fouded items

        PlayerStatistics.currentSpawnChanceMultipier = PlayerStatistics.baseSpawnChanceMultipier;

        PlayerStatistics.currentDetectionRadius = PlayerStatistics.baseDetectionRadius;

        PlayerStatistics.currentSpawnDelay = PlayerStatistics.baseSpawnDelay;

        PlayerScript.player.UpdateBody();
        ItemGeneratorLegacy.instance.Init();
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

        if (!unlocked && playerLevel >= unlockLevel)
        {
            unlocked = true;
            PlayfabStatisticsManager.SaveStat(StatisticsKeys.seekerUnlockedKey, Convert.ToInt32(unlocked));
        }

        return unlocked;
    }

    public void OnItemCollected(string itemId)
    {
        Debug.Log($"item collected: {itemId}");

        itemsCollected++;

        PlayfabStatisticsManager.SaveStat(StatisticsKeys.itemsCollectedKey, itemsCollected);

        if (level < itemsToLevelUp.Length && itemsCollected >= itemsToLevelUp[level + 1])
        {
            LevelUp();  
        }
    }
}
