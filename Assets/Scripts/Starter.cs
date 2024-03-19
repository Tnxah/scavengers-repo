using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    private List<IPrepare> servicesToPrepare = new List<IPrepare>();

    private void Start()
    {
        // Add services to the list
        servicesToPrepare.Add(new JobManager());
        servicesToPrepare.Add(new ItemManager());
        servicesToPrepare.Add(new GeoLocationManager());
        servicesToPrepare.Add(ItemGenerator.instance);
        servicesToPrepare.Add(GPSController.instance);
        servicesToPrepare.Add(LocationPointController.instance);

        // Start the initialization coroutine
        StartCoroutine(InitializeGame());
    }

    private IEnumerator InitializeGame()
    {
        yield return new WaitUntil(() => AccountManager.isLoggedIn);
        yield return new WaitUntil(() => PlayfabStatisticsManager.loaded);

        foreach (IPrepare service in servicesToPrepare)
        {
            bool isCompleted = false;
            bool success = false;
            string errorMsg = null;

            yield return StartCoroutine(service.Prepare((result, error) =>
            {
                success = result;
                errorMsg = error;
                isCompleted = true;
            }));

            yield return new WaitUntil(() => isCompleted);

            if (!success)
            {
                // Handle the error, e.g., log it or show a message
                Debug.LogError($"Service initialization failed: {errorMsg}");
                // Optionally, stop the initialization process
                yield break;
            }
        }

        // All services are prepared, start the game
        StartGame();
    }

    private void StartGame()
    {
        
    }
}
