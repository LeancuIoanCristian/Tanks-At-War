using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Unity.AI.Navigation;



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
    [SerializeField] private int numberOfPoints;
    [SerializeField] private int maxDistance;

    [SerializeField] private bool autoRegenerate = false;
    [SerializeField] private TerrainType[] regions;
    [SerializeField] private int heightMultiplier;
    [SerializeField] private AnimationCurve heightCurve;

    [SerializeField] private float[,] heightMap;
    [SerializeField] private NavMeshSurface navMeshSurface;
    public bool GetAutoRegenerateValue() => autoRegenerate;
    public int GetWidth() => mapWidth;
    public int GetHeight() => mapHeight;
    public float GetHeightMap(int indexX, int indexZ) => heightMap[indexX, indexZ];
    public int GetHeightMultiplier() => heightMultiplier;
    public AnimationCurve GetAnimationCurve() => heightCurve;

    public void GenerateMap()
    {
        if (mapType == DrawMode.Noise)
        {
            GeneratePerlinMap();
        }
        else if (mapType == DrawMode.Worley)
        {
            GenerateWorleyNoiseMap();
        }
        else if (mapType == DrawMode.MixedNoise)
        {
            GenerateMixedNoiseMap();
        }
        else if (mapType == DrawMode.Color)
        {
            GenerateColorMap();
        }
        else if (mapType == DrawMode.ColorWorley)
        {
            GenerateWorleyColorMap();
        }
        else if (mapType == DrawMode.ColorMixedNoise)
        {
            GenerateMixedNoiseColorMap();
        }
        else if (mapType == DrawMode.Mesh)
        {
            GenerateMesh();
        }
        else if (mapType == DrawMode.MeshWorley)
        {
            GenerateWorleyMesh();
        }
        else if (mapType == DrawMode.MeshMixedNoise)
        {
           GenerateMIxedNoiseMesh();
        }
        navMeshSurface.BuildNavMesh();
    }
    public void GeneratePerlinMap()
    {
        float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight,seed, noiseScaleValue, octaves, persistance, lacunarity, offset);
        heightMap = noiseMap;
        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.GenerateMapTexture(TextureGenerator.TextureFromNoiseMap(noiseMap));
    }

    public void GenerateMesh()
    {
        float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScaleValue, octaves, persistance, lacunarity, offset);
        Color[] colorMap = GenerateColorArray(noiseMap);
        heightMap = noiseMap;
        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.CreateMesh(MeshGenerator.CreateMeshTerrain(noiseMap, heightCurve, heightMultiplier), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
    }

    public void GenerateWorleyNoiseMap()
    {
        Vector2[] interestPoints = new Vector2[numberOfPoints];
        float[,] noiseMap = WorleyNoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScaleValue, octaves, persistance, lacunarity, offset, interestPoints, maxDistance);
        heightMap = noiseMap;
        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.GenerateMapTexture(TextureGenerator.TextureFromNoiseMap(noiseMap));
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
        heightMap = noiseMap;
        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.GenerateMapTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
    }
    public void GenerateWorleyColorMap()
    {
        Vector2[] interestPoints = new Vector2[numberOfPoints];
        float[,] noiseMap = WorleyNoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScaleValue, octaves, persistance, lacunarity, offset, interestPoints, maxDistance);
        Color[] colorMap = GenerateColorArray(noiseMap);
        heightMap = noiseMap;
        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.GenerateMapTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
    }
    public void GenerateWorleyMesh()
    {
        Vector2[] interestPoints = new Vector2[numberOfPoints];
        float[,] noiseMap = WorleyNoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScaleValue, octaves, persistance, lacunarity, offset, interestPoints, maxDistance);
        Color[] colorMap = GenerateColorArray(noiseMap);
        heightMap = noiseMap;
        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.CreateMesh(MeshGenerator.CreateMeshTerrain(noiseMap, heightCurve, heightMultiplier), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
    }

    public void GenerateMixedNoiseMap()
    {
        Vector2[] interestPoints = new Vector2[numberOfPoints];
        float[,] noiseMap = MixedNoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScaleValue, octaves, persistance, lacunarity, offset, interestPoints, maxDistance);
        heightMap = noiseMap;
        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.GenerateMapTexture(TextureGenerator.TextureFromNoiseMap(noiseMap));
    }
    public void GenerateMixedNoiseColorMap()
    {
        Vector2[] interestPoints = new Vector2[numberOfPoints];
        float[,] noiseMap = MixedNoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScaleValue, octaves, persistance, lacunarity, offset, interestPoints, maxDistance);
        Color[] colorMap = GenerateColorArray(noiseMap);
        heightMap = noiseMap;
        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.GenerateMapTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
    }
    public void GenerateMIxedNoiseMesh()
    {
        Vector2[] interestPoints = new Vector2[numberOfPoints];
        float[,] noiseMap = MixedNoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScaleValue, octaves, persistance, lacunarity, offset, interestPoints, maxDistance);
        Color[] colorMap = GenerateColorArray(noiseMap);
        heightMap = noiseMap;
        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.CreateMesh(MeshGenerator.CreateMeshTerrain(noiseMap, heightCurve, heightMultiplier), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
    }

    public void Erode()
    {
        GroundEroder.ErodeMap(mapWidth, mapHeight, heightMap);
        Color[] colorMap = GenerateColorArray(heightMap);
        MapDisplayer display = GetComponentInChildren<MapDisplayer>();
        display.CreateMesh(MeshGenerator.CreateMeshTerrain(heightMap, heightCurve, heightMultiplier), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        navMeshSurface.BuildNavMesh();
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
        if (maxDistance < 1)
        {
            maxDistance = 1;
        }
        if (numberOfPoints < 1)
        {
            numberOfPoints = 1;
        }
    }

}

public enum DrawMode
{
    Noise,
    Worley,
    MixedNoise,
    Color,
    ColorWorley,
    ColorMixedNoise,
    Mesh,
    MeshWorley,
    MeshMixedNoise

}