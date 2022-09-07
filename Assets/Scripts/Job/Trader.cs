using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : Job
{
    private const int unlockLevel = 10;

    public Trader()
    {
        type = JobType.Trader;
        level = PlayfabStatisticsManager.GetStat(StatisticsKeys.traderLevelKey);
        unlocked = Convert.ToBoolean(PlayfabStatisticsManager.GetStat(StatisticsKeys.traderUnlockedKey));
    }

    public override void ApplyJobProperties()
    {
        //throw new System.NotImplementedException();
    }

    public override void LevelUp()
    {
        level++;

        ApplyJobProperties();

        //GetReward();

        PlayfabStatisticsManager.SaveStat(StatisticsKeys.traderLevelKey, level);
    }

    public override bool Unlock()
    {
        var playerLevel = PlayfabStatisticsManager.GetStat(StatisticsKeys.playerLevelKey);

        if (!unlocked && playerLevel >= unlockLevel)
        {
            unlocked = true;
            PlayfabStatisticsManager.SaveStat(StatisticsKeys.traderUnlockedKey, Convert.ToInt32(unlocked));
        }

        return unlocked;
    }
}
