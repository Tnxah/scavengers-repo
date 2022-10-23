using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Touch touch;

    public float sensivity = 0.2f;
    public float abovePlayerHeight = 8;

    public Transform playerBody;

    public float maxCamHeight = 36;
    public float minCamHeight = 7;

    private bool doubleTap;

    private void FixedUpdate()
    {
        var lookAtPoint = new Vector3(playerBody.position.x, playerBody.position.y + abovePlayerHeight, playerBody.position.z);

        if (Input.touchCount > 0)
        {

            touch = Input.GetTouch(0);

            if (touch.tapCount == 2)
            {
                doubleTap = true;
            }

            if (!doubleTap && touch.phase.Equals(TouchPhase.Moved))
            {
                var rotationPoint = new Vector3(playerBody.position.x, transform.position.y, playerBody.position.z);

                transform.RotateAround(rotationPoint, Vector3.up, touch.deltaPosition.x * Time.fixedDeltaTime);
            }

            if (doubleTap && touch.phase.Equals(TouchPhase.Moved))
            {
                var height = Mathf.Clamp(touch.deltaPosition.y * Time.fixedDeltaTime + transform.position.y, minCamHeight, maxCamHeight);
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
            }

            if (touch.phase.Equals(TouchPhase.Canceled) || touch.phase.Equals(TouchPhase.Ended))
            {
                doubleTap = false;
            }
        }
        transform.LookAt(lookAtPoint);
    }
}
