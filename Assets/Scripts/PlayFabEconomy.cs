using PlayFab;
using PlayFab.ServerModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabEconomy
{
    public static void IncreaseMoney(int amount)
    {
        AddUserVirtualCurrencyRequest addVCrequest = new AddUserVirtualCurrencyRequest
        {
            Amount = amount,
            PlayFabId = AccountManager.playerId,
            VirtualCurrency = TitleInfo.Currency
        };


        PlayFabServerAPI.AddUserVirtualCurrency(addVCrequest, result => { Debug.Log("Success"); }, error => { Debug.Log("Error"); });

    }
}
