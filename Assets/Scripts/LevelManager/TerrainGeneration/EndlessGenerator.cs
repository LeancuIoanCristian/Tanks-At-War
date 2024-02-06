using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGenerator : MonoBehaviour
{
    [SerializeField] private DrawMode mapType;

    [SerializeField] public const int chunkSize = 241;
    [Range(1, 240)] [SerializeField] private int simplificationLOD;
    [SerializeField] private float noiseScaleValue = 1;
    [SerializeField] private int octaves = 8;
    [Range(0.0f, 1.0f)][SerializeField] private float persistance = 1;
    [SerializeField] private float lacunarity = 1;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset;

    [SerializeField] private bool autoRegenerate = false;
    [SerializeField] private TerrainType[] regions;
    [SerializeField] private int heightMultiplier;
    [SerializeField] private AnimationCurve heightCurve;
    public bool GetAutoRegenerateValue() => autoRegenerate;
    public int GetChunkSize() => chunkSize;

    public void GenerateMap()
    {
        if (mapType == DrawMode.Noise)
        {
            GeneratePerlinMap();
        }
        else if (mapType == DrawMode.Color)
        {
            GenerateColorMap();
        }
        else if (mapType == DrawMode.Mesh)
        {
            GenerateMesh();
        }
    }
    public void GeneratePerlinMap()
    {
        float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(chunkSize, chunkSize, seed, noiseScaleValue, octaves, persistance, lacunarity, offset);

        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.GenerateMapTexture(TextureGenerator.TextureFromNoiseMap(noiseMap));
    }

    public void GenerateMesh()
    {
        float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(chunkSize, chunkSize, seed, noiseScaleValue, octaves, persistance, lacunarity, offset);
        Color[] colorMap = GenerateColorArray(noiseMap);

        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.CreateMesh(MeshGenerator.CreateMeshTerrain(noiseMap, heightCurve, heightMultiplier, simplificationLOD), TextureGenerator.TextureFromColorMap(colorMap, chunkSize, chunkSize));
    }

    private Color[] GenerateColorArray(float[,] noiseMap)
    {
        Color[] colorMap = new Color[chunkSize * chunkSize];

        for (int heightIndex = 0; heightIndex < chunkSize; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < chunkSize; widthIndex++)
            {
                float currentHeight = noiseMap[widthIndex, heightIndex];

                for (int indexRegions = 0; indexRegions < regions.Length; indexRegions++)
                {
                    if (currentHeight <= regions[indexRegions].GetHeight())
                    {
                        colorMap[widthIndex + chunkSize * heightIndex] = regions[indexRegions].GetColor();
                        break;
                    }
                }
            }
        }

        return colorMap;
    }

    public void GenerateColorMap()
    {
        float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(chunkSize, chunkSize, seed, noiseScaleValue, octaves, persistance, lacunarity, offset);
        Color[] colorMap = GenerateColorArray(noiseMap);

        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.GenerateMapTexture(TextureGenerator.TextureFromColorMap(colorMap, chunkSize, chunkSize));
    }

    private void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 1;
        }
        if (heightMultiplier < 1)
        {
            heightMultiplier = 1;
        }
    }

}
