using System.Collections;
using UnityEngine;

namespace Terrain_Generation
{
	public static class FalloffGenerator
	{

		public static float[,] GenerateFalloffMap(int size)
		{
			float[,] map = new float[size, size];

			for (int collum = 0; collum < size; collum++)
			{
				for (int row = 0; row < size; row++)
				{
					float x = collum / (float)size * 2 - 1;
					float y = row / (float)size * 2 - 1;

					float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
					map[collum, row] = Evaluate(value);
				}
			}

			return map;
		}

		static float Evaluate(float value)
		{
			float a = 3;
			float b = 2.2f;

			return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
		}
	}
}
