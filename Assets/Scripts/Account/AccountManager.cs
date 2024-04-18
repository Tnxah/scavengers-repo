using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager
{
    public static bool isLoggedIn;

    public static string playerId;
    public static void Register(string username, string email, string password, Action<bool, string> result)
    {
        var request = new RegisterPlayFabUserRequest();
        request.Username = username;
        request.Email = email;
        request.Password = password;

        PlayFabClientAPI.RegisterPlayFabUser(request, success => {
            result?.Invoke(true, null); 
        }, error => {
            result?.Invoke(false, error.ErrorMessage); 
        });
    }
    public static void Login(string email, string password, Action<bool, string> result)
    {
        var request = new LoginWithEmailAddressRequest();
        request.Email = email;
        request.Password = password;

        PlayFabClientAPI.LoginWithEmailAddress(request, success => {
            
            isLoggedIn = true;
            playerId = success.PlayFabId;
            PlayfabStatisticsManager.LoadStatistics();

            result?.Invoke(true, null);

        }, error => {
            result?.Invoke(false, error.ErrorMessage);
        });
    }

    public static void ResetPassword(string email, Action<bool, string> result)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = TitleInfo.TitleId
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, 
        success => {
            result?.Invoke(true, "Check your email to reset password");
        }, error => {
            result?.Invoke(false, error.ErrorMessage);
        });
    }
}
