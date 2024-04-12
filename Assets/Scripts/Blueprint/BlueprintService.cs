using System;
using System.Reflection;

public class BlueprintService
{
    public static void GrantBlueprint(string blueprintName = null)
    {
        if(blueprintName == null)
        {
            blueprintName = Blueprint.GetRandomBlueprint();
        }

        var amountOfBlueprints = GetAmountOfBlueprints(blueprintName);
        PlayfabUserDataService.SetUserData(blueprintName, (amountOfBlueprints + 1).ToString());
    }

    public static int GetAmountOfBlueprints(string blueprintName)
    {
        return PlayfabUserDataService.UserData.ContainsKey(blueprintName) ? int.Parse(PlayfabUserDataService.UserData[blueprintName]) : 0;
    }
}

public class Blueprint
{
    public const string Pickaxe = "Pickaxe_blueprints";
    public const string Workbrench = "Workbrench_blueprints";

    public static string GetRandomBlueprint()
    {
        // Using reflection to get all public static fields of the Blueprint class
        FieldInfo[] fields = typeof(Blueprint).GetFields(BindingFlags.Public | BindingFlags.Static);

        // Create a random number generator
        Random random = new Random();

        // Pick a random field and return its value
        FieldInfo randomField = fields[random.Next(fields.Length)];
        return (string)randomField.GetValue(null);
    }
}
