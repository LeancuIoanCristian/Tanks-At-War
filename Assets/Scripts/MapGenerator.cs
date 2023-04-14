using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int map_width;
    [SerializeField] private int map_height;
    [SerializeField] private float noise_scale;
    [SerializeField] private int octaves;
    [Range(0,1)]    [SerializeField] private float persistance;
    [SerializeField] private float lacunarity;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset;
    [SerializeField] private TerrainType[] regions;
    public bool auto_update;
    public void GenerateMap()
    {
        float[,] noise_map = PerlinNoiseGenerator.NoiseMapGeneration(map_width, map_height, noise_scale, octaves, persistance, lacunarity, seed, offset);

        for (int height_index = 0; height_index < map_height; height_index++)
        {
            for (int width_index = 0; width_index < map_width; width_index++)
            {

            }
        }

                MapDisplayer display = FindObjectOfType<MapDisplayer>();
        display.DrawNoiseMap(noise_map);
    }

    private void OnValidate()
    {
        if (map_width < 1)
        {
            map_width = 1;
        }
        if (map_height < 1)
        {
            map_height = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 1)
        {
            octaves = 1;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    [SerializeField] private string terrain_name;
    [SerializeField] private float terrain_height;
    [SerializeField] private Color terrain_color;

}