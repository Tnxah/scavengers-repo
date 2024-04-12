using PlayFab.ClientModels;
using System.Collections.Generic;

public static class Inventory
{
    public static Dictionary<string, ItemInstance> items = new Dictionary<string, ItemInstance>();

    public static bool HasTool(string toolId)
    {
        var tool = items.ContainsKey(toolId);

        return tool && ItemManager.TryGetCraftable(toolId).type.Equals(ItemType.TOOL);
    }
    public static bool HasItem(string itemId, int amount = 0)
    {
        var item = items.ContainsKey(itemId);
        if (amount > 0) item = item && items[itemId].RemainingUses >= amount;

        return item;
    }
}
