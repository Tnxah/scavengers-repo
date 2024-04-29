using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Touch touch;
    private float sensitivity = 4f;
    private float abovePlayerHeight = 10f;

    public Transform playerBody;

    private float maxCamHeight = 50f;
    private float minCamHeight = 7f;
    private bool doubleTap;

    private float lastCompassHeading = 0f;

    private bool compass = true;
    public void ActivateCompass() => compass = true;

    private void FixedUpdate()
    {
        var lookAtPoint = new Vector3(playerBody.position.x, playerBody.position.y + abovePlayerHeight, playerBody.position.z);

        // Compass-based rotation
        if (compass)
        {
            float currentHeading = Input.compass.trueHeading;
            float headingDifference = currentHeading - lastCompassHeading;
            if (Mathf.Abs(headingDifference) > 0.3) // Avoid minor rotations
            {
                transform.RotateAround(playerBody.position, Vector3.up, headingDifference);
                lastCompassHeading = currentHeading;
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
