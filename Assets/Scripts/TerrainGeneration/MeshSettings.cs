using UnityEngine;
using System.Collections;

namespace Terrain_Generation
{
	[CreateAssetMenu()]
	public class MeshSettings : UpdatableData
	{

		public const int munber_of_supported_level_of_details = 5;
		public const int number_of_supported_chunk_sizes = 9;
		public const int number_of_supported_flat_shaded_chunk_sizes = 3;
		public static readonly int[] supported_chunk_sizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };

		public float mesh_scale = 2.5f;
		public bool use_flat_shading;

		[Range(0, number_of_supported_chunk_sizes - 1)]
		public int chunk_size_index;
		[Range(0, number_of_supported_flat_shaded_chunk_sizes - 1)]
		public int flatshaded_chunk_size_index;


		// num verts per line of mesh rendered at LOD = 0. Includes the 2 extra verts that are excluded from final mesh, but used for calculating normals
		public int number_of_vertices_per_line
		{
			get
			{
				return supported_chunk_sizes[(use_flat_shading) ? flatshaded_chunk_size_index : chunk_size_index] + 5;
			}
		}

		public float mesh_world_size
		{
			get
			{
				return (number_of_vertices_per_line - 3) * mesh_scale;
			}
		}


	}
}