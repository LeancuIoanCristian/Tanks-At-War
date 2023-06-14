using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain_Generation
{

    public static class PerlinNoiseGenerator
    {
        public enum NormalizeMode
        {
            Local,
            Global            
        }

        public static float[,] NoiseMapGeneration(int width, int height, Vector2 offset, NoiseSettings settings)
        {
            float[,] noise_map = new float[width, height];

            System.Random perlin_noise_random_generator = new System.Random(settings.seed);
            Vector2[] octave_offsets = new Vector2[settings.octaves_numbers];

            float max_hight = 0;
            float amplitude = 1.0f;
            float frequency = 1.0f;

            for (int index = 0; index < settings.octaves_numbers; index++)
            {
                float width_offset = perlin_noise_random_generator.Next(-100000, 100000) + offset.x;
                float height_offset = perlin_noise_random_generator.Next(-100000, 100000) - offset.y;
                octave_offsets[index] = new Vector2(width_offset, height_offset);
                max_hight += amplitude;
                amplitude *= settings.persistance_influence;
            }

            float local_max_noise_height = float.MinValue;
            float local_min_noise_height = float.MaxValue;

            float width_centre = width / 2f;
            float height_centre = height / 2f;

            for (int height_index = 0; height_index < height; height_index++)
            {
                for (int width_index = 0; width_index < width; width_index++)
                {
                    amplitude = 1.0f;
                    frequency = 1.0f;
                    float noise_height = 0.0f;
                    for (int octaves_index = 0; octaves_index < settings.octaves_numbers; octaves_index++)
                    {
                        float width_sample = (width_index - width_centre + octave_offsets[octaves_index].x) / settings.scale * frequency;
                        float height_sample = (height_index - height_centre + octave_offsets[octaves_index].y) / settings.scale * frequency;

                        float perlin_value = Mathf.PerlinNoise(width_sample, height_sample) * 2 - 1;
                        noise_height += perlin_value * amplitude;

                        amplitude *= settings.persistance_influence;
                        frequency *= settings.lacunarity_value;
                    }

                    if (noise_height > local_max_noise_height)
                    {
                        local_max_noise_height = noise_height;
                    }
                    else if (noise_height < local_min_noise_height)
                    {
                        local_min_noise_height = noise_height;
                    }

                    noise_map[width_index, height_index] = noise_height;
                }
            }

            if (settings.normalized_mode == NormalizeMode.Local)
            {
                for (int height_index = 0; height_index < height; height_index++)
                {
                    for (int width_index = 0; width_index < width; width_index++)
                    {
                    
                            noise_map[width_index, height_index] = Mathf.InverseLerp(local_min_noise_height, local_max_noise_height, noise_map[width_index, height_index]);
                    
                    
                    }
                }
            }
            
            return noise_map;
        }
    }

    [System.Serializable]
    public class NoiseSettings
    {
        public PerlinNoiseGenerator.NormalizeMode normalized_mode;

        public float scale = 50;

        public int octaves_numbers = 6;
        [Range(0, 1)]
        public float persistance_influence = .6f;
        public float lacunarity_value = 2;

        public int seed;
        public Vector2 offset;

        public void ValidateValues()
        {
            scale = Mathf.Max(scale, 0.01f);
            octaves_numbers = Mathf.Max(octaves_numbers, 1);
            lacunarity_value = Mathf.Max(lacunarity_value, 1);
            persistance_influence = Mathf.Clamp01(persistance_influence);
        }
    }


}
