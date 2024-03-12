using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float debugLat, debugLon;
    public bool debug;

    public float smoothTime = 0.0f; // Time taken to move from the current position to the target position
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (GPSController.isLocationServiceEnabled && CoordinateConverter.isReady)
        {
            Vector3 convertedPosition = debug? CoordinateConverter.GPSToGamePosition(debugLat, debugLon) : CoordinateConverter.GPSToGamePosition(GPSController.latitude, GPSController.longitude);

            var targetPosition = new Vector3(convertedPosition.x, transform.position.y, convertedPosition.z);

            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
