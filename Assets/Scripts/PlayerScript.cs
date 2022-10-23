using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript player;

    public GameObject body;

    private void Awake()
    {
        if (!player)
        {
            player = this;
        }

        UpdateBody();
    }

    public void UpdateBody()
    {
        body.GetComponent<CapsuleCollider>().radius = PlayerStatistics.currentDetectionRadius;
    }

    public bool IsCloseEnough(Transform obj)
    {
        return Vector3.Distance(transform.position, obj.position) <= PlayerStatistics.interactionRadius;
    }
}
