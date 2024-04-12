using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICraftUnit : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI materials;
    public new TextMeshProUGUI name;

    public void SetInteractable(bool isInteractable)
    {
        GetComponentInChildren<Button>().interactable = isInteractable;
    }

    public void OnCraftButtonClicked()
    {
        WorkbenchService.CraftItem(name.text);
        WorkbenchUIManager.instance.RefreshList();
    }
}
