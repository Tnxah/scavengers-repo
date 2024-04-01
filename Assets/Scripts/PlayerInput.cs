using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Touch touch;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase.Equals(TouchPhase.Began))
            {
                Ray raycast = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(raycast, out hit))
                {
                    IInteractable interactable;
                    if (hit.collider.TryGetComponent(out interactable) && interactable.CanInteract())
                    {
                        if (PlayerScript.player.IsCloseEnough(hit.collider))
                        {
                            interactable.Interact();
                        }
                    }
                }
            }
        }
    }
}