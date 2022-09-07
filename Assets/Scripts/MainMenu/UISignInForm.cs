using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISignInForm : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;

    public Toggle rememberMe;

    public GameObject panel;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.rememberMeKey))
        {
            rememberMe.isOn = bool.Parse(PlayerPrefs.GetString(PlayerPrefsKeys.rememberMeKey));
        }

        if (rememberMe.isOn && PlayerPrefs.HasKey(PlayerPrefsKeys.emailKey) && PlayerPrefs.HasKey(PlayerPrefsKeys.emailKey))
        {
            var email = PlayerPrefs.GetString(PlayerPrefsKeys.emailKey);
            var password = PlayerPrefs.GetString(PlayerPrefsKeys.passwordKey);

            AccountManager.Login(email, password);

            gameObject.SetActive(false);
        }

    }

    public void RegistrationPanel()
    {
        var signUpForm = UIManager.instance.signUpForm;

        panel.SetActive(false);

        signUpForm.panel.SetActive(true);
        signUpForm.PasteCredentials(email.text, password.text);
    }

    public void ResetPassword()
    {
        AccountManager.ResetPassword(email.text);
    }

    public void SignIn()
    {
        RememberMeCheckSave();
        AccountManager.Login(email.text, password.text);
    }

    private void RememberMeCheckSave()
    {
        if (rememberMe.isOn)
        {
            PlayerPrefs.SetString(PlayerPrefsKeys.rememberMeKey, rememberMe.isOn.ToString());
            PlayerPrefs.SetString(PlayerPrefsKeys.emailKey, email.text);
            PlayerPrefs.SetString(PlayerPrefsKeys.passwordKey, password.text);
        }
    }

    public void PasteCredentials(string email, string password)
    {
        this.email.text = email;
        this.password.text = password;
    }
}
