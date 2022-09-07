using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobUi : MonoBehaviour
{
    [SerializeField]
    public JobType type;

    public void Unlock()
    {
        if (JobManager.TryToUnlock(type))
        {
            Destroy(gameObject);
        }
        else
        {
            //(((
        }
    }

    public void ChangeJobTo()
    {
        JobManager.ChangeJobTo(type);
    }
}
