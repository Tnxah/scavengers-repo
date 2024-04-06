using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryUnit : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI count;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI description;

    public string id;

    public void Delete()
    {
        InventoryUIManager.instance.deleteConfirmation.Show(name.text);
    }
}
