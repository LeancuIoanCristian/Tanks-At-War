using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain_Generation
{

    public static class PerlinNoiseGenerator
    {
        public static float[,] NoiseMapGeneration(int width, int height, float scale, int octaves_numbers, float persistance_influence, float lacunarity_value, int seed, Vector2 offset)
        {
            float[,] noise_map = new float[width, height];

            System.Random perlin_noise_random_generator = new System.Random(seed);
            Vector2[] octave_offsets = new Vector2[octaves_numbers];

            for (int index = 0; index < octaves_numbers; index++)
            {
                float width_offset = perlin_noise_random_generator.Next(-100000, 100000) + offset.x;
                float height_offset = perlin_noise_random_generator.Next(-100000, 100000) + offset.y;
                octave_offsets[index] = new Vector2(width_offset, height_offset);
            }

            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            float max_noise_height = float.MinValue;
            float min_noise_height = float.MaxValue;

            float width_centre = width / 2f;
            float height_centre = height / 2f;

            for (int height_index = 0; height_index < height; height_index++)
            {
                for (int width_index = 0; width_index < width; width_index++)
                {
                    float amplitude = 1.0f;
                    float frequency = 1.0f;
                    float noise_height = 0.0f;
                    for (int octaves_index = 0; octaves_index < octaves_numbers; octaves_index++)
                    {
                        float width_sample = (width_index - width_centre) / scale * frequency + octave_offsets[octaves_index].x;
                        float height_sample = (height_index - height_centre) / scale * frequency + octave_offsets[octaves_index].y;

                        float perlin_value = Mathf.PerlinNoise(width_sample, height_sample) * 2 - 1;
                        noise_height += perlin_value * amplitude;

                        amplitude *= persistance_influence;
                        frequency *= lacunarity_value;
                    }

                    if (noise_height > max_noise_height)
                    {
                        max_noise_height = noise_height;
                    }
                    else if (noise_height < min_noise_height)
                    {
                        min_noise_height = noise_height;
                    }

                    noise_map[width_index, height_index] = noise_height;
                }
            }

            for (int height_index = 0; height_index < height; height_index++)
            {
                for (int width_index = 0; width_index < width; width_index++)
                {
                    noise_map[width_index, height_index] = Mathf.InverseLerp(min_noise_height, max_noise_height, noise_map[width_index, height_index]);
                }
            }

            return noise_map;
        }
    }


}
