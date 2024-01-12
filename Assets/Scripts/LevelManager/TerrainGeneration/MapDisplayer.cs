using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LevelManager.TerrainGeneration
{
    class MapDisplayer : MonoBehaviour
    {
        [SerializeField] private Renderer textureRenderer;

        public void GenerateNoiseMapTexture(float[,] noiseMap)
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

            perlinTexture.SetPixels(colorMap);
            perlinTexture.Apply();

            textureRenderer.sharedMaterial.mainTexture = perlinTexture;
            textureRenderer.transform.localScale = new Vector3(width, 1, height);
        }
    }
}
