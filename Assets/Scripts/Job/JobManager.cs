using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    private static Dictionary<JobType, Job> jobs;

    private static Job currentJob;

    public delegate void JobsCallback();
    public static JobsCallback onJobChanged;
    public static JobsCallback onJobUnlock;

    //public static JobManager instance;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //}

    private static void FillJobs()
    {
        jobs = new Dictionary<JobType, Job>();

        jobs.Add(JobType.Trader, new Trader()); //0
        jobs.Add(JobType.Creator, new Creator()); //1
        jobs.Add(JobType.Seeker, new Seeker()); //2
    }

    public static void Init()
    {
        FillJobs();
        LoadJob();
    }

    

    private static void LoadJob()
    {
        currentJob = jobs[(JobType)PlayfabStatisticsManager.GetStat(StatisticsKeys.currentJobKey)];
        currentJob.ApplyJobProperties();
    }

    public static bool TryToUnlock(JobType type)
    {
        var job = jobs[type];

        onJobUnlock?.Invoke();
        return job.Unlock();
    }

    public static Job GetCurrentJob()
    {
        if (currentJob == null)
            LoadJob();

        return currentJob;
    }

    public static bool ChangeJobTo(JobType type)
    {
        if (!jobs[type].IsUnlocked() || currentJob?.GetJobType() == type)
        {
            return false;
        }
        else
        {
            currentJob = jobs[type];
            currentJob.ApplyJobProperties();

            PlayfabStatisticsManager.SaveStat(StatisticsKeys.currentJobKey, (int)currentJob.GetJobType());

            onJobChanged?.Invoke();
            return true;
        }
    }
}
