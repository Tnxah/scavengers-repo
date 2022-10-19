using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics
{
    private static bool firstInit = true;


    //Seeker job
    public static readonly int baseSpawnChance = 100;
    public static float currentSpawnChance = baseSpawnChance;

    // job
    // job


    //Spawn
    public static readonly int spawnRadius = 600;


    //Interaction
    public static readonly int interactionRadius = 50;


    //Inventory
    public static readonly int inventorySize = 80;


    public static void Init()
    {
        if (firstInit)
        {
           // subscribe Init to onLevelUp delegate
        }

        if (!AccountManager.isLoggedIn)
            return;

        var statistics = PlayfabStatisticsManager.statistics;

    }
}
