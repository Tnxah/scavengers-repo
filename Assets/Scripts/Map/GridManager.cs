using UnityEngine;

public class GridManager
{
    private const float cellSize = 1.5f;
    private const float KmPerDegreeLatitude = 111.32f;

    public static Vector2Int GPSToGrid(float latitude, float longitude)
    {
        float latitudeCellSize = cellSize / KmPerDegreeLatitude;
        float longitudeCellSize = cellSize / (KmPerDegreeLatitude * Mathf.Cos(latitude * Mathf.Deg2Rad));

        int x = Mathf.FloorToInt(longitude / longitudeCellSize);
        int y = Mathf.FloorToInt(latitude / latitudeCellSize);

        Debug.Log("GPSToGrid " + x + " " + y);
        return new Vector2Int(x, y);
    }

    public static (float latitude, float longitude) GridToGPSCenter(Vector2Int gridCoordinate)
    {
        // Calculate cell size in degrees
        float latitudeCellSizeDegrees = cellSize / KmPerDegreeLatitude;
        // Placeholder for latitude to calculate longitude size - use the latitude at the bottom of the cell for approximation
        float approximateLatitude = gridCoordinate.y * latitudeCellSizeDegrees;
        float longitudeCellSizeDegrees = cellSize / (KmPerDegreeLatitude * Mathf.Cos(approximateLatitude * Mathf.Deg2Rad));

        // Calculate the center of the cell in GPS coordinates
        float centerLatitude = (gridCoordinate.y + 0.5f) * latitudeCellSizeDegrees;
        float centerLongitude = (gridCoordinate.x + 0.5f) * longitudeCellSizeDegrees;

        Debug.Log("GridToGPSCenter " + centerLatitude + " " + centerLongitude);

        return (centerLatitude, centerLongitude);
    }
}
