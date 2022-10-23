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
                    if (hit.collider.GetComponent<IInteractable>() != null)
                    {
                        hit.collider.GetComponent<IInteractable>().Interact();
                    }
                }
            }
        }
    }
}