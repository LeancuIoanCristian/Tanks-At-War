using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain_Generation
{

    public class TerrainGenerator : MonoBehaviour
    {
		const float player_move_threshold_for_chunk_update = 25f;
		const float sqr_player_move_threshold_for_chunk_update = player_move_threshold_for_chunk_update * player_move_threshold_for_chunk_update;


		public int collider_lod_index;
		public LevelOfDetailInfoStruct[] detail_levels;

		public MeshSettings mesh_settings;
		public HeightMapSettings height_map_settings;
		public TextureData texture_settings;

		public Transform player;
		public Material map_material;

		Vector2 player_position;
		Vector2 player_position_old;

		float mesh_world_size;
		int chunks_visible_in_view_dst;

		Dictionary<Vector2, TerrainChunk> terrain_chunk_dictionary = new Dictionary<Vector2, TerrainChunk>();
		List<TerrainChunk> visible_terrain_chunks = new List<TerrainChunk>();

		void Start()
        {
            MapSetting();
        }

        private void MapSetting()
        {
            texture_settings.ApplyToMaterial(map_material);
            texture_settings.UpdateMeshHeights(map_material, height_map_settings.minHeight, height_map_settings.maxHeight);

            float max_view_dst = detail_levels[detail_levels.Length - 1].GetTreshhold();
            mesh_world_size = mesh_settings.mesh_world_size;
            chunks_visible_in_view_dst = Mathf.RoundToInt(max_view_dst / mesh_world_size);

            UpdateVisibleChunks();
        }

        void Update()
        {
            TurnUpdate();
        }

        private void TurnUpdate()
        {
            player_position = new Vector2(player.position.x, player.position.z);

            if (player_position != player_position_old)
            {
                foreach (TerrainChunk chunk in visible_terrain_chunks)
                {
                    chunk.UpdateCollisionMesh();
                }
            }

            if ((player_position_old - player_position).sqrMagnitude > sqr_player_move_threshold_for_chunk_update)
            {
                player_position_old = player_position;
                UpdateVisibleChunks();
            }
        }

        void UpdateVisibleChunks()
		{
			HashSet<Vector2> already_updated_chunk_coords = new HashSet<Vector2>();
			for (int i = visible_terrain_chunks.Count - 1; i >= 0; i--)
			{
				already_updated_chunk_coords.Add(visible_terrain_chunks[i].coord);
				visible_terrain_chunks[i].UpdateTerrainChunk();
			}

			int current_chunk_x = Mathf.RoundToInt(player_position.x / mesh_world_size);
			int current_chunk_y = Mathf.RoundToInt(player_position.y / mesh_world_size);

			for (int y_offset = -chunks_visible_in_view_dst; y_offset <= chunks_visible_in_view_dst; y_offset++)
			{
				for (int x_offset = -chunks_visible_in_view_dst; x_offset <= chunks_visible_in_view_dst; x_offset++)
				{
					Vector2 viewed_chunk_coord = new Vector2(current_chunk_x + x_offset, current_chunk_y + y_offset);
					if (!already_updated_chunk_coords.Contains(viewed_chunk_coord))
					{
						if (terrain_chunk_dictionary.ContainsKey(viewed_chunk_coord))
						{
							terrain_chunk_dictionary[viewed_chunk_coord].UpdateTerrainChunk();
						}
						else
						{
							TerrainChunk new_chunk = new TerrainChunk(viewed_chunk_coord, height_map_settings, mesh_settings, detail_levels, collider_lod_index, transform, player, map_material);
							terrain_chunk_dictionary.Add(viewed_chunk_coord, new_chunk);
							new_chunk.on_visibility_changed += OnTerrainChunkVisibilityChanged;
							new_chunk.Load();
						}
					}

				}
			}
		}

		void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool is_visible)
		{
			if (is_visible)
			{
				visible_terrain_chunks.Add(chunk);
			}
			else
			{
				visible_terrain_chunks.Remove(chunk);
			}
		}


	}
    public class LevelOfDetailMesh
    {
        private Mesh mesh;
        private bool has_mesh_requested;
        private bool mesh_received;
        private int level_of_detail;
        public event System.Action update_callback;

        public Mesh GetMesh() => mesh;
        public bool MeshChecker() => mesh_received;
        public bool MeshRequestState() => has_mesh_requested;
        public int GetLOD() => level_of_detail;



        public LevelOfDetailMesh(int level_of_detail_passed)
        {
            this.level_of_detail = level_of_detail_passed;
        }

        void OnMeshDataReceived(object mesh_data_object)
        {
            mesh =((MeshData)mesh_data_object).CreateMesh();
            mesh_received = true;

            update_callback();
        }

        public void RequestMesh(HeightMap height_map, MeshSettings mesh_settings)
        {
            has_mesh_requested = true;
            ThreadDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(height_map.values, mesh_settings, level_of_detail), OnMeshDataReceived);
        }

    }

    /// <summary>
    /// Struct that helps in creating and updating the level of detail of chunks
    /// </summary>
    [System.Serializable]
    public struct LevelOfDetailInfoStruct
    {
        [Range(0, MeshSettings.munber_of_supported_level_of_details -1)]
        [SerializeField] int level_of_detail_info;
        [SerializeField] float detailed_view_distance_treshhold;

        public float GetTreshhold() => detailed_view_distance_treshhold;
        public int GetLODInfo() => level_of_detail_info;

        public float sqrVisibleDstTreshhold
        {
            get { return detailed_view_distance_treshhold * detailed_view_distance_treshhold; }
        }
    }

}
