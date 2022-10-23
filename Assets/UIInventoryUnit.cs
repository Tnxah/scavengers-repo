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

    private void Awake()
    {
        PlayFabInventoryService.onGetInventoryCallback += DestroyCheck;
    }

    public void Delete()
    {
        if (int.Parse(count.text) == 1)
        {
            PlayFabInventoryService.ConsumeItem(id);
            return;
        }

        InventoryUIManager.instance.deleteConfirmation.Show(id);
    }

    private void DestroyCheck()
    {
        if (PlayFabInventoryService.items.Find(x => x.DisplayName.Equals(name.text)) == null)
        {
            PlayFabInventoryService.onGetInventoryCallback -= DestroyCheck;
            Destroy(gameObject);
        }
    }
}
