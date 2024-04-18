using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISignFormController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI log;

    [SerializeField]
    private GameObject loginPanel, registerPanel;

    [SerializeField]
    private TMP_InputField username, signUpEmail, signUpPassword, confirmationPassword; 
    
    [SerializeField]
    private TMP_InputField password, email; 

    [SerializeField]
    private Toggle rememberMe;


    public void Start()
    {
        CheckRememberMe();
    }

    private void CheckRememberMe()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.rememberMeKey))
        {
            rememberMe.isOn = bool.Parse(PlayerPrefs.GetString(PlayerPrefsKeys.rememberMeKey));
        }

        if (rememberMe.isOn && PlayerPrefs.HasKey(PlayerPrefsKeys.emailKey) && PlayerPrefs.HasKey(PlayerPrefsKeys.emailKey))
        {
            email.text = PlayerPrefs.GetString(PlayerPrefsKeys.emailKey);
            password.text = PlayerPrefs.GetString(PlayerPrefsKeys.passwordKey);
        }
    }

    public void SignUp()
    {
        if (ValidatePasswords())
        {
            AccountManager.Register(username.text, signUpEmail.text, signUpPassword.text, (isComplete, message) => {
                if (isComplete)
                    log.text = "Registration success";
                else
                    log.text = $"Registration error: {message}";
            });
        }
    }
    public void SignIn()
    {
        AccountManager.Login(email.text, password.text, (isComplete, message) => {
            if (isComplete)
            {
                log.text = "Login success";
                RememberMe();
                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                log.text = $"Login error: {message}";
            }
        });
    }

    private void RememberMe()
    {
        PlayerPrefs.SetString(PlayerPrefsKeys.rememberMeKey, rememberMe.isOn.ToString());
        PlayerPrefs.SetString(PlayerPrefsKeys.emailKey, email.text);
        PlayerPrefs.SetString(PlayerPrefsKeys.passwordKey, password.text);
    }

    public void ResetPassword()
    {
        AccountManager.ResetPassword(email.text, (isComplete, message) => {
            if (isComplete)
                log.text = message;
            else
                log.text = $"Reset password failed: {message}";
        });
    }

    private bool ValidatePasswords()
    {
        var validate = signUpPassword.text.Equals(confirmationPassword.text);

        if (!validate)
            log.text = "Passwords not match";

        return validate;
    }
}
