using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugCoordinatesUI : MonoBehaviour
{
    public TextMeshProUGUI gps, local, compas;

    private void Update()
    {
        gps.text = $"GPS {GPSController.longitude}/{GPSController.latitude}";
        var recounted = CoordinateConverter.GPSToGamePosition(GPSController.latitude, GPSController.longitude);
        local.text = $"Local {recounted.x}/{recounted.z}";
        compas.text = $"Compass {Input.compass.trueHeading} CamRot {Camera.main.transform.eulerAngles.y}";
    }
}
