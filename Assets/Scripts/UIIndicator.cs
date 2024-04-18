using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIndicator : MonoBehaviour
{
    [SerializeField]
    private Sprite OK, Error;
    [SerializeField]
    private Image statusImage;
    public void TurnOff()
    {
        statusImage.gameObject.SetActive(false);
    }

    public void SetStatus(bool status)
    {
        statusImage.gameObject.SetActive(true);
        statusImage.sprite = status ? OK : Error;
    }
}
