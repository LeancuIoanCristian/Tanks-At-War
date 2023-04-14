using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplayer : MonoBehaviour
{
    [SerializeField] private Renderer texture_rendurer;

    public void DrawNoiseMap(float[,] noise_map)
    {
        int width = noise_map.GetLength(0);
        int height = noise_map.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] color_map = new Color[width * height];

        for (int height_index = 0; height_index < height; height_index++)
        {
            for (int width_index = 0; width_index < width; width_index++)
            {
                color_map[height_index * width + width_index] = Color.Lerp(Color.black, Color.white, noise_map[width_index, height_index]);
            }
        }
        texture.SetPixels(color_map);
        texture.Apply();

        texture_rendurer.sharedMaterial.mainTexture = texture;
        texture_rendurer.transform.localScale = new Vector3(width, 1, height);
    }
}
