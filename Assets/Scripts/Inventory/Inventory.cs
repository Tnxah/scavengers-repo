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
}
