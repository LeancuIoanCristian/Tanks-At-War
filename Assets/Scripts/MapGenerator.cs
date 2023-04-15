using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { Noise_Map, Color_Map, Mesh_Map};
    [SerializeField] const int chunk_size = 241;
    [Range(0, 6)] [SerializeField] private int level_of_detail;
    [SerializeField] private DrawMode draw_mode;
    [SerializeField] private float noise_scale;
    [SerializeField] private int octaves;
    [Range(0,1)]    [SerializeField] private float persistance;
    [SerializeField] private float lacunarity;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset;
    [SerializeField] private TerrainType[] regions;
    [SerializeField] private float height_multiplier;
    [SerializeField] private AnimationCurve height_curve;
    public bool auto_update;
    public void GenerateMap()
    {
        float[,] noise_map = PerlinNoiseGenerator.NoiseMapGeneration(chunk_size, chunk_size, noise_scale, octaves, persistance, lacunarity, seed, offset);
        Color[] color_map = new Color[chunk_size * chunk_size];
        for (int height_index = 0; height_index < chunk_size; height_index++)
        {
            for (int width_index = 0; width_index < chunk_size; width_index++)
            {
                float current_height = noise_map[width_index, height_index];
                for (int region_index = 0; region_index < regions.Length; region_index++)
                {
                    if (current_height <= regions[region_index].GetHeight())
                    {
                        color_map[height_index * chunk_size + width_index] = regions[region_index].GetColor();
                        break;
                    }
                }
            }
        }

        MapDisplayer display = FindObjectOfType<MapDisplayer>();
        if (draw_mode == DrawMode.Noise_Map)
        {
            display.DrawTextureMap(TextureGenerator.DrawTextureFromHightMap(noise_map));
        }
        else if (draw_mode == DrawMode.Color_Map)
        {
            display.DrawTextureMap(TextureGenerator.ColorMapTexture(color_map, chunk_size, chunk_size));
        }
        else if (draw_mode == DrawMode.Mesh_Map)
        {
            display.DrawMeshMap(MeshGenerator.GenerateTerrainMesh(noise_map, height_multiplier, height_curve, level_of_detail), TextureGenerator.ColorMapTexture(color_map, chunk_size, chunk_size));
        }

    }

    private void OnValidate()
    {
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
    public float GetHeight() => terrain_height;
    [SerializeField] private Color terrain_color;
    public Color GetColor() => terrain_color;

}