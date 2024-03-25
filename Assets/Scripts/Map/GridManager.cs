using UnityEngine;

public class GridManager
{
    private float cellSize;

    public GridManager(float cellSizeKm)
    {
        this.cellSize = cellSizeKm;
    }

    public Vector2Int GetGridCellFromLocation(float latitude, float longitude)
    {
        int x = Mathf.FloorToInt(longitude / cellSize);
        int y = Mathf.FloorToInt(latitude / cellSize);
        return new Vector2Int(x, y);
    }
}
