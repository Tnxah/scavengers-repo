using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISignUpForm : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField repeatPassword;

    public GameObject panel;

    private string message;

    public void LoginPanel()
    {
        var signInForm = UIManager.instance.signInForm;

        panel.SetActive(false);

        signInForm.panel.SetActive(true);
        signInForm.PasteCredentials(email.text, password.text);
    }

    public void SignUp()
    {
        if (password.text.Equals(repeatPassword.text))
        {
            AccountManager.Register(username.text, email.text, password.text);
        }
        LoginPanel();
    }

    public void PasteCredentials(string email, string password)
    {
        this.email.text = email;
        this.password.text = password;
    }
}
