using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript player;

    public GameObject body;
    public Transform radius;

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
        radius.localScale = new Vector3(PlayerStatistics.interactionRadius, PlayerStatistics.interactionRadius, 1);
    }

    public bool IsCloseEnough(Collider collider)
    {
        return Vector3.Distance(transform.position, collider.ClosestPoint(transform.position)) <= PlayerStatistics.interactionRadius;
    }
}
