using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabTimeService : IPrepare
{
    private static DateTime time;
    private static DateTime localStartDate;

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        var isComplete = false;

        PlayFabServerAPI.GetTime(new PlayFab.ServerModels.GetTimeRequest(), result => { time = result.Time; isComplete = true; }, error => { time = DateTime.Now; isComplete = true; Debug.LogError("Error ResourcePoint get time"); });

        yield return new WaitUntil(() => isComplete);

        localStartDate = DateTime.Now;

        onComplete?.Invoke(true, null);
    }

    public static DateTime CurrentTime()
    {
        TimeSpan timeDifference = DateTime.Now - localStartDate;
        return time + timeDifference;
    }
    public static bool WasYesterday(DateTime time)
    {
        return CurrentTime().Day > time.Day;
    }
}
