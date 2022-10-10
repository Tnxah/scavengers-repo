using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugService
{

    public static void Log(string message)
    {
        Debug.Log($"{Time.time} - (message) = {message} (Scavengers log)");
    }
}