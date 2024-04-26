using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private int width = 256;  // Width of the terrain chunk
    private int depth = 256;  // Depth of the terrain chunk
    private float height = 1.5f;  // Maximum elevation of the terrain

    private float scale = 20;

    private Vector3 terrainStartPosition;

    private float xOffset, yOffset;

    private Terrain terrain;
    private TerrainData terrainData;

    void Start()
    {
        terrainStartPosition = transform.position;
        terrain = GetComponent<Terrain>();

        terrainData = terrain.terrainData;
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, height, depth);
        terrainData.SetHeights(0, 0, GenerateHeights());
        terrainData.terrainLayers[0].tileOffset = new Vector2(transform.position.x, transform.position.z);
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                heights[x, y] = CalculateHaights(x, y);
            }
        }
        return heights;
    }

    float CalculateHaights(int x, int y)
    {
        float xCoord = (float)x / width * scale + transform.position.z / 12.8f;
        //float xCoord = (float)x / width * scale + xOffset;
        float yCoord = (float)y / depth * scale + transform.position.x / 12.8f;
        //float yCoord = (float)y / depth * scale + yOffset;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    void FixedUpdate()
    {
        Vector3 playerPosition = PlayerScript.player.transform.position;
        Vector3 terrainPosition = terrain.transform.position;

        // Check if the player is close to the edge of the terrain
        if (Mathf.Abs(playerPosition.x - terrainPosition.x - width / 2) > width / 3 ||
            Mathf.Abs(playerPosition.z - terrainPosition.z - depth / 2) > depth / 3)
        {
            LoadNewChunk(playerPosition);
        }
    }

    void LoadNewChunk(Vector3 playerPosition)
    {
        // Calculate new terrain position based on player position
        Vector3 newTerrainPosition = playerPosition;
        newTerrainPosition.x = Mathf.Floor(playerPosition.x - width / 2);
        newTerrainPosition.y = -height / 2;
        newTerrainPosition.z = Mathf.Floor(playerPosition.z - depth / 2);

        terrain.transform.position = newTerrainPosition;
        terrainStartPosition = newTerrainPosition;
        GenerateTerrain();  // Regenerate terrain at the new position
    }
}