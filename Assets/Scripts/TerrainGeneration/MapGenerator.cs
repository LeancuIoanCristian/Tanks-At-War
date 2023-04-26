using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

namespace Terrain_Generation
{

    public class MapGenerator : MonoBehaviour
    {
        public enum DrawMode { Noise_Map, Color_Map, Mesh_Map };
        [SerializeField] public const int chunk_size = 241;

        [Tooltip("Changes the level of detail of the mesh. The lower the value the higher the leve of detial of the mesh")]
        [Range(0, 6)] [SerializeField] private int editor_level_of_detail;

        [Tooltip("Changes the drawing mode in between perlin noise generation, colored perlin noise and mesh generation")]
        [SerializeField] private DrawMode draw_mode;

        [Tooltip("Zooms in and out in the noise, making it easy to get more simple or complex regions")]
        [SerializeField] private float noise_scale;

        [Tooltip("Modify the impact of one gradient value to the others")]
        [SerializeField] private int octaves;

        [Tooltip("Changes the level of detail the noise has. The higher the value the higher/ more chaotic/detailed the noise is")]
        [Range(0, 1)] [SerializeField] private float persistance;

        [Tooltip("Smoothens the transition between the gradient values")]
        [SerializeField] private float lacunarity;

        [Tooltip("Generates an entire new noise, a noise that was generated on a seed value will remain the same regardless if ou change to another one and then you come back")]
        [SerializeField] private int seed;

        [Tooltip("Use the offset to move through the noise on X and Z axis")]
        [SerializeField] private Vector2 offset;

        [Tooltip("Portions of the gradient that can be colored in accordance to a color value assigned to a value range")]
        [SerializeField] private TerrainType[] regions;

        [Tooltip("Changes the height of the vertexes in the mesh")]
        [SerializeField] private float height_multiplier;

        [Tooltip("Adjusts the behaviour of the height multiplier, giving more natural looks or can be customized to give more of your desired aestetic")]
        [SerializeField] private AnimationCurve height_curve;

        [Tooltip("Updates the model after any change in the variables")]
        public bool auto_update;

        Queue<MapDataQueueInfo<MapData>> map_data_info_queue = new Queue<MapDataQueueInfo<MapData>>();
        Queue<MapDataQueueInfo<MeshData>> mesh_data_info_queue = new Queue<MapDataQueueInfo<MeshData>>();


        public int GetLOD() => editor_level_of_detail;
        public int GetChunkSize() => chunk_size;

        /// <summary>
        /// //Request a MapData object and create a new thread for it
        /// </summary>
        /// <param name="callback"></param>
        public void RequestDataMap(Action<MapData> callback) 
        {
            ThreadStart thread_start = delegate
            {
                ThreadMapData(callback);
            };

            new Thread(thread_start).Start();
        }

        /// <summary>
        /// Request a MapData object and the level of detail to create a thread for a new mesh for a chunk
        /// </summary>
        /// <param name="map_data"></param>
        /// <param name="callback"></param>
        /// <param name="level_of_detail_requested"></param>
        public void RequestDataMesh(MapData map_data, Action<MeshData> callback, int level_of_detail_requested)
        {
            ThreadStart thread_start = delegate
            {
                ThreadMeshData(map_data, callback, level_of_detail_requested);
            };

            new Thread(thread_start).Start();
        }

        /// <summary>
        /// Adds the MapData thread to the thread queue locking it from being accessed by other threads
        /// </summary>
        /// <param name="callback"></param>
        void ThreadMapData(Action<MapData> callback)
        {
            MapData map_data = GenerateMapData();
            lock (map_data_info_queue)
            {
                map_data_info_queue.Enqueue(new MapDataQueueInfo<MapData>(callback, map_data));
            }
        }


        /// <summary>
        ///  Adds the MeshData thread to the thread queue locking it from being accessed by other threads
        /// </summary>
        /// <param name="map_data"></param>
        /// <param name="callback"></param>
        /// <param name="level_of_detail_requested"></param>
        void ThreadMeshData(MapData map_data, Action<MeshData> callback,  int level_of_detail_requested)
        {
            MeshData mesh_data = MeshGenerator.GenerateTerrainMesh(map_data.height_map, height_multiplier, height_curve, editor_level_of_detail);
            lock (mesh_data_info_queue)
            {
                mesh_data_info_queue.Enqueue(new MapDataQueueInfo<MeshData>(callback, mesh_data));
            }
            
        }

        void Update()
        {
            if (map_data_info_queue.Count > 0)
            {
                for (int index = 0; index < map_data_info_queue.Count; index++)
                {
                    MapDataQueueInfo<MapData> thread_info = map_data_info_queue.Dequeue();
                    thread_info.callback(thread_info.parameter);
                }
            }
            if (mesh_data_info_queue.Count > 0)
            {
                for (int index = 0; index < mesh_data_info_queue.Count; index++)
                {
                    MapDataQueueInfo<MeshData> thread_info = mesh_data_info_queue.Dequeue();
                    thread_info.callback(thread_info.parameter);
                }
            }
        }

        /// <summary>
        /// Generates the perlin noise
        /// </summary>
        /// <returns></returns>
        public MapData GenerateMapData()
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

            return new MapData(noise_map, color_map);
        }

        /// <summary>
        /// draws/creates:
        //+ the perlin noise visuals on a texture - DrawMode.Noise_Map
        //+ the perlin noise visuals on a texture that get colored in corelation with the regions values - DrawMode.Color_Map
        //+ the mesh with the colored map - DrawMode.Mesh_Map
        /// </summary>
        /// <param name="noise_map"></param>
        /// <param name="color_map"></param>
        private void MapDrawingModeChoice(float[,] noise_map, Color[] color_map)
        {
            MapData map_data = GenerateMapData();
            MapDisplayer display = FindObjectOfType<MapDisplayer>();
            if (draw_mode == DrawMode.Noise_Map)
            {
                display.DrawTextureMap(TextureGenerator.DrawTextureFromHightMap(map_data.height_map));
            }
            else if (draw_mode == DrawMode.Color_Map)
            {
                display.DrawTextureMap(TextureGenerator.ColorMapTexture(map_data.color_map, chunk_size, chunk_size));
            }
            else if (draw_mode == DrawMode.Mesh_Map)
            {
                display.DrawMeshMap(MeshGenerator.GenerateTerrainMesh(map_data.height_map, height_multiplier, height_curve, editor_level_of_detail), TextureGenerator.ColorMapTexture(map_data.color_map, chunk_size, chunk_size));
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

    /// <summary>
    /// Terrain type, refered as regions as well, influences color of  triangles at diferent perlin noise heights 
    /// </summary>
    [System.Serializable]
    public struct TerrainType
    {
        [SerializeField] private string terrain_name;
        [SerializeField] private float terrain_height;
        public float GetHeight() => terrain_height;
        [SerializeField] private Color terrain_color;
        public Color GetColor() => terrain_color;

    }

    /// <summary>
    /// Stores the perlin noise and the color map for the terrain chunks
    /// </summary>
    public struct MapData
    {
        public readonly float[,] height_map;
        public readonly Color[] color_map;

        public MapData(float[,] height_map, Color[] color_map)
        {
            this.height_map = height_map;
            this.color_map = color_map;
        }
    }

    /// <summary>
    /// Queue for diferent variables used for threading and callbacks of different/ certain paramters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct MapDataQueueInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapDataQueueInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }

}