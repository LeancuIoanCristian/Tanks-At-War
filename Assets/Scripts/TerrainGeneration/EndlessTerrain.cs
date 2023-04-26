using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain_Generation
{

    public class EndlessTerrain : MonoBehaviour
    {
        //maximum distance where the chunks are generated and visible to the player
        [SerializeField] private static float max_view_diatance;

        [Tooltip("Determine the level of mesh detail you want for your chunk in direct corelation with the distance from the player. The greater the level of detail the highet the simplicity of the mesh")]
        [SerializeField] private LevelOfDetailInfoStruct[] detail_level;

        [Tooltip("Drop in here the gameobject that will move through the level")]
        [SerializeField] private Transform player;

        [SerializeField] static MapGenerator map_generator;
        [SerializeField] private static Vector2 player_position;
        [SerializeField] private int chunk_size;
        [SerializeField] private int visible_chunks_in_view_distance;
        [SerializeField] private Material map_material;

        [SerializeField] Dictionary<Vector2, TerrainChunk> terrain_chunks_dictionary = new Dictionary<Vector2, TerrainChunk>();

        //list of chunks that will be set to inactive as they are out of the visual range of the player
        [SerializeField] List<TerrainChunk> chunks_to_be_deactivated = new List<TerrainChunk>();

        private void Start()
        {
            max_view_diatance = detail_level[detail_level.Length - 1].GetTreshhold();
            map_generator = FindObjectOfType<MapGenerator>();
            chunk_size = MapGenerator.chunk_size - 1;
            visible_chunks_in_view_distance = Mathf.RoundToInt(max_view_diatance / chunk_size);
        }

        void Update()
        {
            player_position = new Vector2(player.position.x, player.position.z);
            UpdateVisibleChunks();
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
                        terrain_chunks_dictionary.Add(near_chunk_coord, new TerrainChunk(near_chunk_coord, chunk_size, transform, map_material, detail_level));
                    }
                }
            }
        }

        public class TerrainChunk
        {
            [SerializeField] private GameObject mesh_obj;
            [SerializeField] private Vector2 position;
            [SerializeField] private Bounds bounds;
            [SerializeField] private MapData map_data;

            [SerializeField] private LevelOfDetailInfoStruct[] detail_level_chunk;
            [SerializeField] private LevelOfDetailMesh[] detail_mesh_chunk;

            [SerializeField] private MapData map_data_chunk;
            bool map_data_received;
            int default_chunk_detail_index = -1;

            [SerializeField] private MeshRenderer mesh_renderer;
            [SerializeField] private MeshFilter mesh_filter;

            public TerrainChunk(Vector2 coords, int size, Transform parent, Material material, LevelOfDetailInfoStruct[] detail_level)
            {
                this.detail_level_chunk = detail_level;
                position = coords * size;
                Vector3 position_3D = new Vector3(position.x, 0f, position.y);

                mesh_obj = new GameObject("Chunk");
                mesh_renderer = mesh_obj.AddComponent<MeshRenderer>();
                mesh_filter = mesh_obj.AddComponent<MeshFilter>();
                mesh_renderer.material = material;

                mesh_obj.transform.position = position_3D;
                mesh_obj.transform.parent = parent;
                SetVisible(false);

                detail_mesh_chunk = new LevelOfDetailMesh[detail_level.Length];
                for (int index = 0; index < detail_level.Length; index++)
                {
                    detail_mesh_chunk[index] = new LevelOfDetailMesh(detail_level[index].GetLODInfo());
                }

                map_generator.RequestDataMap(OnReceivedData);
                mesh_obj.layer = 6;
            }

            //retrives the MapData from the thread 
            void OnReceivedData(MapData map_data)
            {
                this.map_data_chunk = map_data;
                map_generator.RequestDataMesh(map_data, OnMeshDataReceived, map_generator.GetLOD());
                map_data_received = true;
            }

            void OnMeshDataReceived(MeshData mesh_data)
            {
                mesh_filter.mesh = mesh_data.CreateMesh();
            }

            public void UpdateTerrainChunk()
            {
                if (map_data_received)
                {
                    float distance_from_edge = Mathf.Sqrt(bounds.SqrDistance(player_position));
                    bool visible = distance_from_edge <= max_view_diatance;
                    if (visible)
                    { 
                        MeshAllocation(distance_from_edge);
                        SetVisible(visible);
                    }

                    SetVisible(visible);
                }  
                
               
            }

            private void MeshAllocation(float distance_from_edge)
            {
                int detail_index = 0;
                detail_index = CheckForDetailLevelIndex(distance_from_edge, detail_index);

                if (detail_index != default_chunk_detail_index)
                {
                    LevelOfDetailMesh level_of_detail_chunk_mesh = detail_mesh_chunk[detail_index];
                    if (level_of_detail_chunk_mesh.MeshChecker())
                    {
                        mesh_filter.mesh = level_of_detail_chunk_mesh.GetMesh();
                        default_chunk_detail_index = detail_index;
                    }
                    else if (!level_of_detail_chunk_mesh.MeshRequestState())
                    {
                        level_of_detail_chunk_mesh.RequestMesh(map_data);
                    }
                }
            }

            private int CheckForDetailLevelIndex(float distance_from_edge, int detail_index)
            {
                for (int index = 0; index < detail_level_chunk.Length - 1; index++)
                {
                    if (distance_from_edge > detail_level_chunk[index].GetTreshhold())
                    {
                        detail_index = index + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                return detail_index;
            }

            public void SetVisible(bool visible)
            {
                mesh_obj.SetActive(visible);
            }

            public bool IsVisible()
            {
                return mesh_obj.activeSelf;
            }
        }
        class LevelOfDetailMesh
        {
            Mesh mesh;
            bool mesh_requested;
            bool mesh_received;
            int level_of_detail;

            public Mesh GetMesh() => mesh;
            public bool MeshChecker() => mesh_received;
            public bool MeshRequestState() => mesh_requested;

            

            public LevelOfDetailMesh(int level_of_detail_passed)
            {
                this.level_of_detail = level_of_detail_passed;
            }

            void OnMeshDataReceived(MeshData mesh_data)
            {
                mesh = mesh_data.CreateMesh();
                mesh_received = true;
            }

            public void RequestMesh(MapData map_data)
            {
                mesh_requested = true;
                map_generator.RequestDataMesh(map_data, OnMeshDataReceived, level_of_detail);
            }

        }

        /// <summary>
        /// Struct that helps in creating and updating the level of detail of chunks
        /// </summary>
        [System.Serializable]
        public struct LevelOfDetailInfoStruct
        {
            [SerializeField] int level_of_detail_info;
            [SerializeField] float detailed_view_distance_treshhold;

            public float GetTreshhold() => detailed_view_distance_treshhold;
            public int GetLODInfo() => level_of_detail_info;
        }
    }  

}
