using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D perlinTexture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];
        for (int colorIndexHeigth = 0; colorIndexHeigth < height; colorIndexHeigth++)
        {
            for (int colorIndexWidth = 0; colorIndexWidth < width; colorIndexWidth++)
            {
                colorMap[colorIndexHeigth * width + colorIndexWidth] = Color.Lerp(Color.black, Color.white, noiseMap[colorIndexWidth, colorIndexHeigth]);
            }
        }

        
        return TextureFromColorMap(colorMap, width, height);
    }
}

