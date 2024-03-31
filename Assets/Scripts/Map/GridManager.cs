using UnityEngine;

public class GridManager
{
    private const float cellSize = .8f;
    private const float KmPerDegreeLatitude = 111.32f;

    private static float latitudeOffset;
    private static float longitudeOffset;

    private static int seed;
    public static int Seed
    {
        private get { return seed; }
        set
        {
            seed = value;
            latitudeOffset = GenerateOffset(seed, 0) * 1f;
            longitudeOffset = GenerateOffset(seed, 1) * 1f;
        }
    }

    public static Vector2Int GPSToGrid(float latitude, float longitude)
    {
        // Convert latitude and longitude to a "world position" considering the offsets
        float worldLatitude = latitude + latitudeOffset;
        float worldLongitude = longitude + longitudeOffset;

        // Then, convert the "world position" to grid coordinates
        float latitudeCellSize = cellSize / KmPerDegreeLatitude;
        float longitudeCellSize = cellSize / (KmPerDegreeLatitude * Mathf.Cos(latitude * Mathf.Deg2Rad));

        int x = Mathf.FloorToInt(worldLongitude / longitudeCellSize);
        int y = Mathf.FloorToInt(worldLatitude / latitudeCellSize);

        Debug.Log($"GPSToGrid: {x}, {y}");
        return new Vector2Int(x, y);
    }

    public static (float latitude, float longitude) GridToGPSCenter(Vector2Int gridCoordinate)
    {
        float latitudeCellSizeDegrees = cellSize / KmPerDegreeLatitude;

        // Approximate center latitude for better longitude cell size calculation
        float centerLatitudeApprox = (gridCoordinate.y + 0.5f) * latitudeCellSizeDegrees - latitudeOffset;
        float longitudeCellSizeDegrees = cellSize / (KmPerDegreeLatitude * Mathf.Cos(centerLatitudeApprox * Mathf.Deg2Rad));

        float centerLatitude = centerLatitudeApprox + latitudeOffset;
        float centerLongitude = ((gridCoordinate.x + 0.5f) * longitudeCellSizeDegrees) - longitudeOffset;

        Debug.Log($"GridToGPSCenter: {centerLatitude}, {centerLongitude}");

        return (centerLatitude, centerLongitude);
    }

    // Adjusted to generate relatively small offsets
    private static float GenerateOffset(int seed, int type)
    {
        System.Random rand = new System.Random(seed + type); // Ensure different seeds for latitude/longitude
        // Generate a small offset, e.g., within +/- 0.5 km range
        return (float)(rand.NextDouble() - 0.5) * (type == 0 ? 0.01f : 0.01f); // Adjusted for more sensible shifts
    }
}
