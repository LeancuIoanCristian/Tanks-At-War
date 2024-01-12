using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LevelManager.TerrainGeneration
{
    public static class NoiseMapGenerator
    {
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            if (scale <= 0)
            {
                scale = 0.001f;
            }

            for (int heightIndex = 0; heightIndex < mapHeight; mapHeight++)
            {
                for (int widthIndex = 0; widthIndex < mapWidth; widthIndex++)
                {
                    float sampleWidth = widthIndex / scale;
                    float sampleHeight = heightIndex/scale;

                    float perlinValue = Mathf.PerlinNoise(sampleWidth, sampleHeight);
                    noiseMap[heightIndex, widthIndex] = perlinValue;
                }
            }

            return noiseMap;
        }
    }
}
