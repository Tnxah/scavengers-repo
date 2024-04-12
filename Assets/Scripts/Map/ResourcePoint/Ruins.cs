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
            //Open Workbrench
        }
        else
        {
            PlayfabUserDataService.SetUserData(resourcePointId, "");
            BlueprintService.GrantBlueprint();
        }
    }
}
