using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        DebugService.Log($"trigger enter {other.name}");
        var hider = other?.GetComponent<IHidable>();
        if (hider != null)
        {
            hider.Unhide();
        }
    }
}
