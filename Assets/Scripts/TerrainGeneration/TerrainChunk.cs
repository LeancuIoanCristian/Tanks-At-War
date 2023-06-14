using UnityEngine;

namespace Terrain_Generation
{
	public class TerrainChunk
	{
		const float collider_generation_distance_threshold = 5;
		public event System.Action<TerrainChunk, bool> on_visibility_changed;
		public Vector2 coord;

		GameObject mesh_object;
		Vector2 sample_centre;
		Bounds bounds;

		MeshRenderer mesh_renderer;
		MeshFilter mesh_filter;
		MeshCollider mesh_collider;

		LevelOfDetailInfoStruct[] detail_levels;
		LevelOfDetailMesh[] lod_meshes;
		int collider_lod_index;

		HeightMap height_map;
		bool height_map_received;
		int previous_lod_index = -1;
		bool has_set_collider;
		float max_view_dst;

		HeightMapSettings height_map_settings;
		MeshSettings mesh_settings;
		Transform player;

		public TerrainChunk(Vector2 coord, HeightMapSettings height_map_settings, MeshSettings mesh_settings, LevelOfDetailInfoStruct[] detail_levels, int collider_lod_index, Transform parent, Transform player, Material material)
		{
			this.coord = coord;
			this.detail_levels = detail_levels;
			this.collider_lod_index = collider_lod_index;
			this.height_map_settings = height_map_settings;
			this.mesh_settings = mesh_settings;
			this.player = player;

			sample_centre = coord * mesh_settings.mesh_world_size / mesh_settings.mesh_scale;
			Vector2 position = coord * mesh_settings.mesh_world_size;
			bounds = new Bounds(position, Vector2.one * mesh_settings.mesh_world_size);


			mesh_object = new GameObject("Terrain Chunk");
			mesh_renderer = mesh_object.AddComponent<MeshRenderer>();
			mesh_filter = mesh_object.AddComponent<MeshFilter>();
			mesh_collider = mesh_object.AddComponent<MeshCollider>();
			mesh_renderer.material = material;

			mesh_object.transform.position = new Vector3(position.x, 0, position.y);
			mesh_object.transform.parent = parent;
			SetVisible(false);

			lod_meshes = new LevelOfDetailMesh[detail_levels.Length];
			for (int i = 0; i < detail_levels.Length; i++)
			{
				lod_meshes[i] = new LevelOfDetailMesh(detail_levels[i].GetLODInfo());
				lod_meshes[i].update_callback += UpdateTerrainChunk;
				if (i == collider_lod_index)
				{
					lod_meshes[i].update_callback += UpdateCollisionMesh;
				}
			}

			max_view_dst = detail_levels[detail_levels.Length - 1].GetTreshhold();

		}

		public void Load()
		{
			ThreadDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap(mesh_settings.number_of_vertices_per_line, mesh_settings.number_of_vertices_per_line, height_map_settings, sample_centre), OnHeightMapReceived);
		}



		void OnHeightMapReceived(object height_map_object)
		{
			this.height_map = (HeightMap)height_map_object;
			height_map_received = true;

			UpdateTerrainChunk();
		}

		Vector2 playerPosition
		{
			get
			{
				return new Vector2(player.position.x, player.position.z);
			}
		}


		public void UpdateTerrainChunk()
		{
			if (height_map_received)
			{
				float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPosition));

				bool wasVisible = IsVisible();
				bool visible = viewerDstFromNearestEdge <= max_view_dst;

				if (visible)
				{
					int lodIndex = 0;

					for (int i = 0; i < detail_levels.Length - 1; i++)
					{
						if (viewerDstFromNearestEdge > detail_levels[i].GetTreshhold())
						{
							lodIndex = i + 1;
						}
						else
						{
							break;
						}
					}

					if (lodIndex != previous_lod_index)
					{
						LevelOfDetailMesh lod_mesh = lod_meshes[lodIndex];
						if (lod_mesh.MeshChecker())
						{
							previous_lod_index = lodIndex;
							mesh_filter.mesh = lod_mesh.GetMesh();
						}
						else if (!lod_mesh.MeshRequestState())
						{
							lod_mesh.RequestMesh(height_map, mesh_settings);
						}
					}


				}

				if (wasVisible != visible)
				{

					SetVisible(visible);
					if (on_visibility_changed != null)
					{
						on_visibility_changed(this, visible);
					}
				}
			}
		}

		public void UpdateCollisionMesh()
		{
			if (!has_set_collider)
			{
				float sqrDstFromViewerToEdge = bounds.SqrDistance(playerPosition);

				if (sqrDstFromViewerToEdge < detail_levels[collider_lod_index].sqrVisibleDstTreshhold)
				{
					if (!lod_meshes[collider_lod_index].MeshRequestState())
					{
						lod_meshes[collider_lod_index].RequestMesh(height_map, mesh_settings);
					}
				}

				if (sqrDstFromViewerToEdge < collider_generation_distance_threshold * collider_generation_distance_threshold)
				{
					if (lod_meshes[collider_lod_index].MeshChecker())
					{
						mesh_collider.sharedMesh = lod_meshes[collider_lod_index].GetMesh();
						has_set_collider = true;
					}
				}
			}
		}

		public void SetVisible(bool visible)
		{
			mesh_object.SetActive(visible);
		}

		public bool IsVisible()
		{
			return mesh_object.activeSelf;
		}
	}
}
