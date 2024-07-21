using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Touch touch;
    private float sensitivity = 4f;
    private float abovePlayerHeight = 10f;
    private float distanceToPlayer = 35.5f;

    public Transform playerBody;

    private float maxCamHeight = 80f;
    private float minCamHeight = 8f;
    private bool doubleTap;

    private bool compass = false;
    public void ActivateCompass() => compass = true;

    private void FixedUpdate()
    {
        var lookAtPoint = new Vector3(playerBody.position.x, playerBody.position.y + abovePlayerHeight, playerBody.position.z);

        // Compass-based rotation
        if (compass)
        {
            float currentHeading = Input.compass.trueHeading;
            var normalDifference = currentHeading - transform.eulerAngles.y;
            float throughZeroDifference = currentHeading > 180 ? -(360 - currentHeading + transform.eulerAngles.y) : currentHeading + 360 - transform.eulerAngles.y;
            float headingDifference = Mathf.Abs(normalDifference) > 180 ? throughZeroDifference : normalDifference;

            if (Mathf.Abs(headingDifference) > 4.5f) // Avoid minor rotations
            {
                transform.RotateAround(playerBody.position, Vector3.up, headingDifference * Time.fixedDeltaTime * 10f);
            }
        }

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.tapCount == 2)
            {
                doubleTap = true;
            }

            if (!doubleTap && touch.phase == TouchPhase.Moved)
            {
                // Rotate around the player on horizontal swipes
                compass = false;
                float rotationAmount = touch.deltaPosition.x * sensitivity * Time.fixedDeltaTime;
                transform.RotateAround(playerBody.position, Vector3.up, rotationAmount);
            }

            if (doubleTap && touch.phase == TouchPhase.Moved)
            {
                // Adjust camera height on vertical swipes
                float height = Mathf.Clamp(touch.deltaPosition.y * Time.fixedDeltaTime + transform.position.y, minCamHeight, maxCamHeight);
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
            }

            if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
            {
                doubleTap = false;
            }
        }

        transform.LookAt(lookAtPoint); // Ensure the camera always looks at the point above the player
    }
}
