using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager
{
    public delegate void AccountCallback();
    public static AccountCallback onSignUpCallback;
    public static AccountCallback onSignInCallback;

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
        request.TitleId = "67F9D";

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnResetPasswordSuccess, OnResetPasswordError);
    }

    private static void OnSignInSuccess(LoginResult result)
    {
        PlayfabStatisticsManager.LoadStatistics();

        onSignInCallback?.Invoke();
    }
    private static void OnSignInError(PlayFabError error)
    {

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
