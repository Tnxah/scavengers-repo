using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Prepare());
    }

    private IEnumerator Prepare()
    {
        yield return new WaitUntil(() => AccountManager.isLoggedIn);
        yield return new WaitUntil(() => PlayfabStatisticsManager.loaded);

        JobManager.Prepare();

        ItemManager.Prepare();
        yield return new WaitUntil(() => ItemManager.isReady());

        ItemGenerator.instance.Prepare();
        yield return new WaitUntil(() => ItemGenerator.instance.isReady());
    }
}
