using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UISignInForm signInForm;
    public UISignUpForm signUpForm;



    public static UIManager instance;


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }   
    }


}
