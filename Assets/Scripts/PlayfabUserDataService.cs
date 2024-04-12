using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabUserDataService : IPrepare
{
    private const string countPostfix = "_count", timePostfix = "_time";

    public static Dictionary<string, string> UserData = new Dictionary<string, string>();

    public static void SetUserData(string key, string value)
    {
        UserData[key] = value;

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = UserData
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public static int GetCount(string resourcePointId)
    {
        return UserData.ContainsKey(resourcePointId + countPostfix) ? int.Parse(UserData[resourcePointId + countPostfix]) : 0;
    }

    private static void GetUserData(Action<bool, string> onComplete)
    {

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = AccountManager.playerId,
            Keys = null
        }, result => {
            if (result.Data == null)
            {
                onComplete?.Invoke(true, "no user data found");
            }
            else 
            {
                foreach (var data in result.Data)
                {
                    UserData.Add(data.Key, data.Value.Value);
                }
                onComplete?.Invoke(true, null);
            }
        }, (error) => {
            onComplete?.Invoke(false, "something went wrong with user data: " + error.ErrorMessage);
        });
    }

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        GetUserData(onComplete);
        yield break;
    }
}
