using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager
{
    public static event Action onSignUpCallback;
    public static event Action onSignInCallback;

    public static bool isLoggedIn;

    public static string playerId;
    public static void Register(string username, string email, string password)
    {
        var request = new RegisterPlayFabUserRequest();
        request.Username = username;
        request.Email = email;
        request.Password = password;

        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignUpSuccess, OnSignUpError);
    }
    public static void Login(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest();
        request.Email = email;
        request.Password = password;

        PlayFabClientAPI.LoginWithEmailAddress(request, OnSignInSuccess, OnSignInError);
    }

    public static void ResetPassword(string email)
    {
        var request = new SendAccountRecoveryEmailRequest();
        request.Email = email;
        request.TitleId = TitleInfo.TitleId;

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnResetPasswordSuccess, OnResetPasswordError);
    }

    private static void OnSignInSuccess(LoginResult result)
    {
        isLoggedIn = true;

        Debug.Log("Successful login");

        playerId = result.PlayFabId;

        onSignInCallback?.Invoke();

        PlayFabInventoryService.GetUserInventory();

        PlayfabStatisticsManager.LoadStatistics();
    }
    private static void OnSignInError(PlayFabError error)
    {

        Debug.Log("Unsuccessful login: " + error.ErrorMessage);
    }
    private static void OnSignUpSuccess(RegisterPlayFabUserResult result)
    {

        onSignUpCallback?.Invoke();
    }
    private static void OnSignUpError(PlayFabError error)
    {

    }
    private static void OnResetPasswordSuccess(SendAccountRecoveryEmailResult result)
    {

    }
    private static void OnResetPasswordError(PlayFabError error)
    {

    }
}
