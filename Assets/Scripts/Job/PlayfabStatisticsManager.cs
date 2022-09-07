using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabStatisticsManager
{
    public static List<StatisticValue> statistics = statistics = new List<StatisticValue>();

    public static bool loaded = false;

    public static void LoadStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    public static void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        statistics = result.Statistics;

        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);

        loaded = true;
    }

    public static int GetStat(string statisticKey)
    {
        //if (!loaded)
            LoadStatistics();

        var statistic = statistics.Find(stat => stat.StatisticName == statisticKey);

        if (statistic == null)
        {

            SaveStat(statisticKey, 0);
            return 0;
        }
        Debug.Log(statisticKey);
        return statistic.Value;
    }

    public static IEnumerator GetStat()
    {
        yield return new WaitUntil( () => true);
    }

    public static void SaveStat(string statisticKey, int value = 0)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate { StatisticName = statisticKey, Value = value },
            }
        },
        result => { Debug.Log("User statistics updated " + $"({statisticKey})"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }
}
