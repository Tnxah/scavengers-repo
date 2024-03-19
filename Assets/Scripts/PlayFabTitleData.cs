using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public class PlayFabTitleData
{
    public static void GetTitleData(string key, Action<string> onSuccess, Action onFail)
    {
        var request = new GetTitleDataRequest { Keys = new List<string> { key } };
        PlayFabClientAPI.GetTitleData(request, result =>
        {
            if (result.Data != null && result.Data.ContainsKey(key))
            {
                onSuccess?.Invoke(result.Data[key]);
            }
            else
            {
                onFail?.Invoke();
            }
        }, error =>
        {
            onFail?.Invoke();
        });
    }
}
