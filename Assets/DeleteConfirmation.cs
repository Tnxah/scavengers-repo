using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeleteConfirmation : MonoBehaviour
{
    public string id;
    public TMP_InputField count;

    public void Delete()
    {
        PlayFabInventoryService.ConsumeItem(id, int.Parse(count.text));
        Hide();
    }

    public void Cancel()
    {
        Hide();
    }

    private void Hide()
    {
        id = "";
        count.text = "";
        gameObject.SetActive(false);
    }
    public void Show(string id)
    {
        this.id = id;
        gameObject.SetActive(true);
    }
}
