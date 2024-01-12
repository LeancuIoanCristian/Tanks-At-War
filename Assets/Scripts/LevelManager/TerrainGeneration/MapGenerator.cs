using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LevelManager.TerrainGeneration
{
    class MapGenerator :MonoBehaviour
    {
        [Range(1, 200)]
        [SerializeField] private int mapWidth = 10;
        [Range(1, 200)]
        [SerializeField] private int mapHeight = 10;
        [Range(0.001f, 50f)]
        [SerializeField] private float noiseScaleValue = 1;
       

        public void GeneratePerlinMap()
        {
            float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(mapWidth, mapHeight, noiseScaleValue);

            MapDisplayer display = GetComponentInChildren<MapDisplayer>();
            display.GenerateNoiseMapTexture(noiseMap);
        }

        
    }
}
