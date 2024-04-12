using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruins : ResourcePoint
{
    public override bool CanInteract()
    {
        return true;
    }

    public override void Interact()
    {
        if (PlayfabUserDataService.UserData.ContainsKey(resourcePointId))
        {
            WorkbenchUIManager.instance.OpenCloseInventory();
        }
        else
        {
            PlayfabUserDataService.SetUserData(resourcePointId, "");
            BlueprintService.GrantBlueprint();
        }
    }
}
