using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePointFactory
{
    private int globalSeed;

    public ResourcePointFactory(int globalSeed)
    {
        this.globalSeed = globalSeed;
    }

    public string DetermineFeatureType(int cellHash)
    {
        if (cellHash % 3 == 0) return "Metal Field";
        if (cellHash % 3 == 1) return "Coal Field";
        if (cellHash % 3 == 2) return "Water Source";
        return "Ruins";
    }

    public int GenerateGridCellHash(Vector2Int cell)
    {
        return cell.x.GetHashCode() ^ cell.y.GetHashCode() ^ globalSeed;
    }

    public void PlaceFeatureInCell(Vector2Int cell)
    {
        int hash = GenerateGridCellHash(cell);
        string featureType = DetermineFeatureType(hash);

        // Logic to place the feature on the map, based on its type
        Debug.Log($"Placing {featureType} in grid cell {cell}.");
    }
}
