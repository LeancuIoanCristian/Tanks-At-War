using UnityEngine;
using System.Collections;
using System.Linq;


namespace Terrain_Generation
{
	[CreateAssetMenu()]
	public class TextureData: UpdatableData
    {
		const int texture_size = 512;
		const TextureFormat texture_format = TextureFormat.RGB565;

		public Layer[] layers;

		float saved_min_height;
		float saved_max_height;

		public void ApplyToMaterial(Material material)
		{

			material.SetInt("layerCount", layers.Length);
			material.SetColorArray("baseColours", layers.Select(x => x.tint).ToArray());
			material.SetFloatArray("baseStartHeights", layers.Select(x => x.start_height).ToArray());
			material.SetFloatArray("baseBlends", layers.Select(x => x.blend_strength).ToArray());
			material.SetFloatArray("baseColourStrength", layers.Select(x => x.tint_strength).ToArray());
			material.SetFloatArray("baseTextureScales", layers.Select(x => x.texture_scale).ToArray());
			Texture2DArray texturesArray = GenerateTextureArray(layers.Select(x => x.texture).ToArray());
			material.SetTexture("baseTextures", texturesArray);

			UpdateMeshHeights(material, saved_min_height, saved_max_height);
		}

		public void UpdateMeshHeights(Material material, float min_height, float max_height)
		{
			saved_min_height = min_height;
			saved_max_height = max_height;

			material.SetFloat("min_height", min_height);
			material.SetFloat("max_height", max_height);
		}

		Texture2DArray GenerateTextureArray(Texture2D[] textures)
		{
			Texture2DArray texture_array = new Texture2DArray(texture_size, texture_size, textures.Length, texture_format, true);
			for (int i = 0; i < textures.Length; i++)
			{
				texture_array.SetPixels(textures[i].GetPixels(), i);
			}
			texture_array.Apply();
			return texture_array;
		}

		[System.Serializable]
		public class Layer
		{
			public Texture2D texture;
			public Color tint;
			[Range(0, 1)]
			public float tint_strength;
			[Range(0, 1)]
			public float start_height;
			[Range(0, 1)]
			public float blend_strength;
			public float texture_scale;
		}

	}
}
