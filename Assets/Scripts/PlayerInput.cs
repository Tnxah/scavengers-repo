using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public TextMeshProUGUI text;
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
                //text.text = "Touch began " + touch.position;
                if (Physics.Raycast(raycast, out hit))
                {
                    if (hit.collider.GetComponent<IInteractable>() != null)
                    {
                    print("Something Hit " + hit.collider.gameObject.name);
                        hit.collider.GetComponent<IInteractable>().Interact();
                    }
                }
            }
        }
    }
}