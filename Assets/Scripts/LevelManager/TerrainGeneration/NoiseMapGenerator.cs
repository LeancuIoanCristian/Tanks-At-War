using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class NoiseMapGenerator
{
    /// <summary>
    /// The function will create the perlin noise height map that will be later used for mesh generation of the terrain
    /// </summary>
    /// <param name="mapWidth"> mapWidth and mapHeight will be used to determine the size of the terrain space wise</param>
    /// <param name="mapHeight"></param>
    /// <param name="scale"> scale is used to determine the complexity of the noise we want to create</param>
    /// <param name="octaves"> octaves will determine the number of regions we will have constrained by the perlin noise height</param>
    /// <param name="persistance"> persistance will determine how much 2 or more octaves influence one other</param>
    /// <param name="lacunarity"> lacunarity will smothen out the differences between 2 or more octaves, by avereging the area between regions, making a smother transitoin in between them</param>
    /// <returns></returns>
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight,int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffsets = new Vector2[octaves];

        for (int octavesIndex = 0; octavesIndex < octaves; octavesIndex++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octavesOffsets[octavesIndex] = new Vector2(offsetX, offsetY);
        }

            if (scale <= 0)
        {
            scale = 0.001f;
        }

        float maxPoint = float.MinValue;
        float minPoint = float.MaxValue;

        float centreWidth = mapWidth / 2f;
        float centreHeight = mapHeight / 2f;

        for (int heightIndex = 0; heightIndex < mapHeight; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < mapWidth; widthIndex++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for (int octavesIndex = 0; octavesIndex < octaves; octavesIndex++)
                {
                    float sampleWidth = (widthIndex - centreWidth) / scale * frequency + octavesOffsets[octavesIndex].x;
                    float sampleHeight = (heightIndex - centreHeight) / scale * frequency + octavesOffsets[octavesIndex].y;

                    float perlinValue = Mathf.PerlinNoise(sampleWidth, sampleHeight) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                   
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxPoint)
                {
                    maxPoint = noiseHeight;
                }
                else if (noiseHeight < minPoint)
                {
                    minPoint = noiseHeight;                    
                }
                noiseMap[widthIndex, heightIndex] = noiseHeight;
            }
        }

        for (int heightIndex = 0; heightIndex < mapHeight; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < mapWidth; widthIndex++)
            {
                noiseMap[widthIndex, heightIndex] = Mathf.InverseLerp(minPoint, maxPoint, noiseMap[widthIndex, heightIndex]);
            }
        }

        return noiseMap;
    }
}

