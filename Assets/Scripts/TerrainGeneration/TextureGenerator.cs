using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain_Generation
{


    public static class TextureGenerator
    {
        public static Texture2D ColorMapTexture(Color[] color_map, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels(color_map);
            texture.Apply();
            return texture;
        }
        public static Texture2D DrawTextureFromHightMap(HeightMap height_map)
        {
            int width = height_map.values.GetLength(0);
            int height = height_map.values.GetLength(1);


            Color[] color_map = new Color[width * height];

            for (int height_index = 0; height_index < height; height_index++)
            {
                for (int width_index = 0; width_index < width; width_index++)
                {
                    color_map[height_index * width + width_index] = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(height_map.minValue, height_map.maxValue, height_map.values[width_index, height_index]));
                }
            }
            return ColorMapTexture(color_map, width, height);
        }
    }

}
