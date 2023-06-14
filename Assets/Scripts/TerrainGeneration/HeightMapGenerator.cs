using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Terrain_Generation
{
	public static class HeightMapGenerator
	{

		public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings, Vector2 sample_centre)
		{
			float[,] values = PerlinNoiseGenerator.NoiseMapGeneration(width, height,  sample_centre, settings.noise_settings);

			AnimationCurve height_curve_threadsafe = new AnimationCurve(settings.height_curve.keys);

			float min_value = float.MaxValue;
			float max_value = float.MinValue;

			for (int collum = 0; collum < width; collum++)
			{
				for (int row = 0; row < height; row++)
				{
					values[collum, row] *= height_curve_threadsafe.Evaluate(values[collum, row]) * settings.height_multiplier;

					if (values[collum, row] > max_value)
					{
						max_value = values[collum, row];
					}
					if (values[collum, row] < min_value)
					{
						min_value = values[collum, row];
					}
				}
			}

			return new HeightMap(values, min_value, max_value);
		}

	}

	public struct HeightMap
	{
		public readonly float[,] values;
		public readonly float minValue;
		public readonly float maxValue;

		public HeightMap(float[,] values, float minValue, float maxValue)
		{
			this.values = values;
			this.minValue = minValue;
			this.maxValue = maxValue;
		}
	}
}
