using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job
{
    protected JobType type;

    protected int level;

    public bool unlocked;

    public JobType GetJobType()
    {
        return type;
    }

    public abstract bool Unlock();


    public abstract void ApplyJobProperties();
    public abstract void LevelUp();

    public bool IsUnlocked()
    {
        return unlocked;
    }
}
