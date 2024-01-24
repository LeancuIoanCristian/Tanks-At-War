using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;



class MapGenerator : MonoBehaviour
{
    [SerializeField] private DrawMode mapType;

    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField] private float noiseScaleValue = 1;
    [SerializeField] private int octaves = 8;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float persistance = 1;
    [SerializeField] private float lacunarity = 1;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset;

    [SerializeField] private bool autoRegenerate = false;
    [SerializeField] private TerrainType[] regions;
    [SerializeField] private int heightMultiplier;
    [SerializeField] private AnimationCurve heightCurve;
    public bool GetAutoRegenerateValue() => autoRegenerate;

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
        float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight,seed, noiseScaleValue, octaves, persistance, lacunarity, offset);

        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.GenerateMapTexture(TextureGenerator.TextureFromNoiseMap(noiseMap));
    }

    public void GenerateMesh()
    {
        float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScaleValue, octaves, persistance, lacunarity, offset);
        Color[] colorMap = GenerateColorArray(noiseMap);

        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.CreateMesh(MeshGenerator.CreateMeshTerrain(noiseMap, heightCurve, heightMultiplier), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
    }

    private Color[] GenerateColorArray(float[,] noiseMap)
    {
        Color[] colorMap = new Color[mapWidth * mapHeight];

        for (int heightIndex = 0; heightIndex < mapHeight; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < mapWidth; widthIndex++)
            {
                float currentHeight = noiseMap[widthIndex, heightIndex];

                for (int indexRegions = 0; indexRegions < regions.Length; indexRegions++)
                {
                    if (currentHeight <= regions[indexRegions].GetHeight())
                    {
                        colorMap[widthIndex + mapWidth * heightIndex] = regions[indexRegions].GetColor();
                        break;
                    }
                }
            }
        }

        return colorMap;
    }

    public void GenerateColorMap()
    {
        float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScaleValue, octaves, persistance, lacunarity, offset);
        Color[] colorMap = GenerateColorArray(noiseMap);

        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.GenerateMapTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
    }

    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
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

public enum DrawMode
{
    Noise,
    Color,
    Mesh
}