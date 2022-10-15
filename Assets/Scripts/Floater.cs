using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    // User Inputs
    private float degreesPerSecond = 15.0f;
    private float amplitude = 0.2f;
    private float frequency = 0.3f;
    // Position Storage Variables
    Vector3 tempPos = new Vector3();

    float startY;

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        startY = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.fixedDeltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = new Vector3(transform.position.x, startY, transform.position.z);
        tempPos.y += (Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude) + amplitude;

        transform.position = tempPos;
    }
}
