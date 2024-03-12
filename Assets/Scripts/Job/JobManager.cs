using System;
using System.Collections;
using System.Collections.Generic;

public class JobManager : IPrepare
{
    private static Dictionary<JobType, Job> jobs;

    private static Job currentJob;

    public static event Action onJobChanged;
    public static event Action onJobUnlock;

    private static void FillJobs()
    {
        if (jobs != null) return;

        jobs = new Dictionary<JobType, Job>();

        jobs.Add(JobType.Trader, new Trader()); //0
        jobs.Add(JobType.Creator, new Creator()); //1
        jobs.Add(JobType.Seeker, new Seeker()); //2
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
            currentJob.CancelJobProperties();

            currentJob = jobs[type];
            currentJob.ApplyJobProperties();

            PlayfabStatisticsManager.SaveStat(StatisticsKeys.currentJobKey, (int)currentJob.GetJobType());

            onJobChanged?.Invoke();
            return true;
        }
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        try
        {
            FillJobs();
            LoadJob();

            // If successful
            Console.WriteLine($"Prepare JobManager result: {true} {null}");
            onComplete?.Invoke(true, null);
        }
        catch (Exception ex)
        {
            // On error
            Console.WriteLine($"Prepare JobManager result: {false} {ex.Message}");
            onComplete?.Invoke(false, ex.Message);
        }

        yield break;
    }
}
