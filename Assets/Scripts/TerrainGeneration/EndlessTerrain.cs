using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain_Generation
{

    public class EndlessTerrain : MonoBehaviour
    {
        [SerializeField] private const float scale = 1f;
        //maximum distance where the chunks are updated/generated and visible to the player
        [SerializeField] private static float max_view_distance;
        [SerializeField] private const float chunk_update_distance = 25f;
        [SerializeField] private const float range_of_chunk_update_distance = 25f;

        [Tooltip("Determine the level of mesh detail you want for your chunk in direct corelation with the distance from the player. The greater the level of detail the highet the simplicity of the mesh")]
        [SerializeField] private LevelOfDetailInfoStruct[] detail_level;

        [Tooltip("Drop in here the gameobject that will move through the level")]
        [SerializeField] private Transform player;

        [SerializeField] static MapGenerator map_generator;
        [SerializeField] private static Vector2 player_position;
        [SerializeField] private Vector2 old_player_position;
        [SerializeField] private int chunk_size;
        [SerializeField] private int visible_chunks_in_view_distance;
        [SerializeField] private Material map_material;

        [SerializeField] Dictionary<Vector2, TerrainChunk> terrain_chunks_dictionary = new Dictionary<Vector2, TerrainChunk>();

        //list of chunks that will be set to inactive as they are out of the visual range of the player
        [SerializeField] static List<TerrainChunk> chunks_to_be_deactivated = new List<TerrainChunk>();

        private void Start()
        {
            MapSetUp();
        }

        private void MapSetUp()
        {
            max_view_distance = detail_level[detail_level.Length - 1].GetTreshhold();
            map_generator = FindObjectOfType<MapGenerator>();
            chunk_size = MapGenerator.chunk_size - 1;
            visible_chunks_in_view_distance = Mathf.RoundToInt(max_view_distance / chunk_size);
            UpdateVisibleChunks();
        }

        void Update()
        {
            MapTurnUpdate();
        }

        private void MapTurnUpdate()
        {
            player_position = new Vector2(player.position.x, player.position.z)/scale;
            if ((old_player_position - player_position).sqrMagnitude > range_of_chunk_update_distance)
            {
                old_player_position = player_position;
                UpdateVisibleChunks();
            }            
        }

        void UpdateVisibleChunks()
        {
           
            for (int index = 0; index < chunks_to_be_deactivated.Count; index++)
            {
                chunks_to_be_deactivated[index].SetVisible(false);
            }
            chunks_to_be_deactivated.Clear();

            int current_chunk_x_position = Mathf.RoundToInt(player_position.x / chunk_size);
            int current_chunk_y_position = Mathf.RoundToInt(player_position.y / chunk_size);

            for (int y_offset = -visible_chunks_in_view_distance; y_offset <= visible_chunks_in_view_distance; y_offset++)
            {
                for (int x_offset = -visible_chunks_in_view_distance; x_offset <= visible_chunks_in_view_distance; x_offset++)
                {
                    Vector2 near_chunk_coord = new Vector2(current_chunk_x_position + x_offset, current_chunk_y_position + y_offset);

                    if (terrain_chunks_dictionary.ContainsKey(near_chunk_coord))
                    {                        
                        terrain_chunks_dictionary[near_chunk_coord].UpdateTerrainChunk();
                        if (terrain_chunks_dictionary[near_chunk_coord].IsVisible())
                        {
                            chunks_to_be_deactivated.Add(terrain_chunks_dictionary[near_chunk_coord]);
                        }
                    }
                    else
                    {
                        //terrain_chunks_dictionary.Add(near_chunk_coord, new TerrainChunk(near_chunk_coord, chunk_size, transform, map_material, detail_level));
                    }
                }
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
