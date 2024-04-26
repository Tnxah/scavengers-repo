using UnityEngine;

public class GridManager
{
    private const float cellSize = .3f;
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
        float worldLatitude = latitude + latitudeOffset;
        float worldLongitude = longitude + longitudeOffset;

        float latitudeCellSize = cellSize / KmPerDegreeLatitude;
        float longitudeCellSize = cellSize / (KmPerDegreeLatitude * Mathf.Cos(latitude * Mathf.Deg2Rad));

        int x = Mathf.FloorToInt(worldLongitude / longitudeCellSize);
        int y = Mathf.FloorToInt(worldLatitude / latitudeCellSize);

        return new Vector2Int(x, y);
    }

    public static (float latitude, float longitude) GridToGPSCenter(Vector2Int gridCoordinate)
    {
        float latitudeCellSizeDegrees = cellSize / KmPerDegreeLatitude;

        float centerLatitudeApprox = (gridCoordinate.y + 0.5f) * latitudeCellSizeDegrees - latitudeOffset;
        float longitudeCellSizeDegrees = cellSize / (KmPerDegreeLatitude * Mathf.Cos(centerLatitudeApprox * Mathf.Deg2Rad));

        float centerLatitude = centerLatitudeApprox + latitudeOffset;
        float centerLongitude = ((gridCoordinate.x + 0.5f) * longitudeCellSizeDegrees) - longitudeOffset;

        return (centerLatitude, centerLongitude);
    }

    private static float GenerateOffset(int seed, int type)
    {
        System.Random rand = new System.Random(seed + type);
        return (float)(rand.NextDouble() - 0.5) * (type == 0 ? 0.01f : 0.01f);
    }
}
