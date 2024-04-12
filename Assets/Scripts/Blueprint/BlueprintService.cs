using System;
using System.Reflection;

public class BlueprintService
{
    public static void GrantBlueprint(string blueprintName = null)
    {
        if(blueprintName == null)
        {
            blueprintName = GetRandomBlueprint();
        }

        var amountOfBlueprints = GetAmountOfBlueprints(blueprintName);
        PlayfabUserDataService.SetUserData(blueprintName, (amountOfBlueprints + 1).ToString());
    }

    public static int GetAmountOfBlueprints(string blueprintName)
    {
        return PlayfabUserDataService.UserData.ContainsKey(blueprintName) ? int.Parse(PlayfabUserDataService.UserData[blueprintName]) : 0;
    }

    private static string GetRandomBlueprint()
    {
        return ItemManager.GetRandomCraftableKey();
    }
}
