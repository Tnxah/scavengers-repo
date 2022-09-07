using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : Job
{
    public Creator()
    {
        type = JobType.Creator;
        level = PlayfabStatisticsManager.GetStat(StatisticsKeys.creatorLevelKey);
        unlocked = Convert.ToBoolean(PlayfabStatisticsManager.GetStat(StatisticsKeys.creatorUnlockedKey));
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

        PlayfabStatisticsManager.SaveStat(StatisticsKeys.creatorLevelKey, level);
    }

    public override bool Unlock()
    {
        unlocked = true;
        PlayfabStatisticsManager.SaveStat(StatisticsKeys.creatorUnlockedKey, Convert.ToInt32(unlocked));
        return unlocked;
    }
}
