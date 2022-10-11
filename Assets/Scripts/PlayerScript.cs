using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript player;

    private void Awake()
    {
        if (!player)
        {
            player = this;
        }
    }

    public bool IsCloseEnough(Transform obj)
    {
        return Vector3.Distance(transform.position, obj.position) <= PlayerStatistics.interactionRadius;
    }
}
