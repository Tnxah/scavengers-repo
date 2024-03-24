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
                    print("RAYCAST");
                    if (hit.collider.GetComponent<IInteractable>() != null)
                    {
                        print("RAYCAST !NULL");
                        if (Physics.Raycast(PlayerScript.player.transform.position, (hit.collider.transform.position - PlayerScript.player.transform.position).normalized, out hit, Mathf.Infinity) && PlayerScript.player.IsCloseEnough(hit.point))
                        {
                            print("ONE MORE RAYCAST");
                            Debug.DrawRay(PlayerScript.player.transform.position, (hit.collider.transform.position - PlayerScript.player.transform.position).normalized * 500, Color.red, 5f);
                            hit.collider.GetComponent<IInteractable>().Interact();
                        }
                    }
                }
            }
        }
    }
}