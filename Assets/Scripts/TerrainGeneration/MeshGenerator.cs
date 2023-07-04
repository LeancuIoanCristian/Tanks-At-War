using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain_Generation
{

    public static class MeshGenerator
    {
        public static MeshData GenerateTerrainMesh(float[,] height_map, MeshSettings mesh_settings, int level_of_detail)
        {
            int increment_of_detail = (level_of_detail == 0) ? 1 :  level_of_detail * 2;
            int verteces_per_line = mesh_settings.number_of_vertices_per_line;

            Vector2 top_left = new Vector2(-1, 1) * mesh_settings.mesh_world_size / 2f;           

            MeshData mesh_data = new MeshData(verteces_per_line, increment_of_detail, mesh_settings.use_flat_shading);
            
            int[,] vertex_index_map = new int[verteces_per_line, verteces_per_line];
            int mesh_vertex_index = 0;
            int out_of_mesh_vertex_index = 0;

            for (int height_index = 0; height_index < verteces_per_line; height_index += increment_of_detail)
            {
                for (int width_index = 0; width_index < verteces_per_line; width_index += increment_of_detail)
                {
                    bool is_out_of_mesh_mertex = height_index == 0 || height_index == verteces_per_line - 1 || width_index == 0 || width_index == verteces_per_line - 1;
                    bool is_skipped_vertex = width_index > 2 && width_index < verteces_per_line - 3 && height_index > 2 && height_index < verteces_per_line - 3 && ((width_index - 2) % increment_of_detail != 0 || (height_index - 2) % increment_of_detail != 0);
                    if (is_out_of_mesh_mertex)
                    {
                        vertex_index_map[width_index, height_index] = out_of_mesh_vertex_index;
                        out_of_mesh_vertex_index--;
                    }
                    else if (!is_skipped_vertex)
                    {
                        vertex_index_map[width_index, height_index] = mesh_vertex_index;
                        mesh_vertex_index++;
                    }
                }
            }

            for (int height_index = 0; height_index < verteces_per_line; height_index ++)
            {
                for (int width_index = 0; width_index < verteces_per_line; width_index ++)
                {
                    bool is_skipped_vertex = width_index > 2 && width_index < verteces_per_line - 3 && height_index > 2 && height_index < verteces_per_line - 3 && ((width_index - 2) % increment_of_detail != 0 || (height_index - 2) % increment_of_detail != 0);

                    if (!is_skipped_vertex)
                    {
                        bool is_out_of_mesh_vertex = height_index == 0 || height_index == verteces_per_line - 1 || width_index == 0 || width_index == verteces_per_line - 1;
                        bool is_mesh_edge_vertex = (height_index == 1 || height_index == verteces_per_line - 2 || width_index == 1 || width_index == verteces_per_line - 2) && !is_out_of_mesh_vertex;
                        bool is_main_vertex = (width_index - 2) % increment_of_detail == 0 && (height_index - 2) % increment_of_detail == 0 && !is_out_of_mesh_vertex && !is_mesh_edge_vertex;
                        bool is_edge_connection_vertex = (height_index == 2 || height_index == verteces_per_line - 3 || width_index == 2 || width_index == verteces_per_line - 3) && !is_out_of_mesh_vertex && !is_mesh_edge_vertex && !is_main_vertex;

                        int vertex_index = vertex_index_map[width_index, height_index];
                        Vector2 percent = new Vector2(width_index - 1, height_index - 1) / (increment_of_detail - 3);
                        Vector2 vertex_position_2D = top_left + new Vector2(percent.x, -percent.y) * mesh_settings.mesh_world_size;
                        float height = height_map[width_index, height_index];

                        if (is_edge_connection_vertex)
                        {
                            bool is_vertical = width_index == 2 || width_index == increment_of_detail - 3;
                            int dst_to_main_vertex_a = ((is_vertical) ? height_index - 2 : width_index - 2) % increment_of_detail;
                            int dst_to_main_vertex_b = increment_of_detail - dst_to_main_vertex_a;
                            float dst_percent_from_a_to_b = dst_to_main_vertex_a / (float)increment_of_detail;
                            if (((is_vertical) ? width_index : width_index + dst_to_main_vertex_b )> height_map.GetLength(0))
                            {
                                Debug.Log("From dimension 0.  Vertex B: " + dst_to_main_vertex_b + " Vertex A : " + dst_to_main_vertex_a + " Width Index : " + width_index);
                               
                            }
                            if (((is_vertical) ? width_index : width_index + dst_to_main_vertex_b) > height_map.GetLength(1))
                            {
                                Debug.Log("From dimension 1.  Vertex B: " + dst_to_main_vertex_b + " Vertex A : " + dst_to_main_vertex_a + " Width Index : " + width_index);

                            }

                            float height_main_vertex_a = height_map[(is_vertical) ? width_index : width_index - dst_to_main_vertex_a, (is_vertical) ? height_index - dst_to_main_vertex_a : height_index];
                            float height_main_vertex_b = height_map[(is_vertical) ? width_index : width_index + dst_to_main_vertex_b, (is_vertical) ? height_index + dst_to_main_vertex_b : height_index];

                            

                            height = height_main_vertex_a * (1 - dst_percent_from_a_to_b) + height_main_vertex_b * dst_percent_from_a_to_b;
                        }

                        mesh_data.AddVertex(new Vector3(vertex_position_2D.x, height, vertex_position_2D.y), percent, vertex_index);

                        bool create_triangle = width_index < verteces_per_line - 1 && height_index < verteces_per_line - 1 && (!is_edge_connection_vertex || (width_index != 2 && height_index != 2));

                        if (create_triangle)
                        {
                            int currentIncrement = (is_main_vertex && width_index != verteces_per_line - 3 && height_index != verteces_per_line - 3) ? increment_of_detail : 1;

                            int a = vertex_index_map[width_index, height_index];
                            int b = vertex_index_map[width_index + currentIncrement, height_index];
                            int c = vertex_index_map[width_index, height_index + currentIncrement];
                            int d = vertex_index_map[width_index + currentIncrement, height_index + currentIncrement];
                            mesh_data.AddTriangle(a, d, c);
                            mesh_data.AddTriangle(d, a, b);
                        }
                    }
                }
            }

            mesh_data.ProcessMesh();





            return mesh_data;
        }
    }

    public class MeshData
    {
        [SerializeField] private Vector3[] vertices;
        [SerializeField] private Vector2[] uvs;
        [SerializeField] private int[] triangles;
        [SerializeField] private int triangles_index;
        Vector3[] baked_normals;
        Vector3[] out_of_mesh_vertices;
        int[] out_of_mesh_triangles;
        int out_of_mesh_triangle_index;
        bool use_flatshading = false;

        public Vector3[] GetVertices() => vertices;
        public Vector2[] GetUVs() => uvs;
        public int[] GetTriangle() => triangles;

        public MeshData(int number_of_vertices_per_line, int lod_increment, bool use_flatshading)
        {
            int number_mesh_edge_vertices = (number_of_vertices_per_line - 2) * 4 - 4;
            int number_edge_connection_vertices = (lod_increment - 1) * (number_of_vertices_per_line - 5) / lod_increment * 4;
            int number_main_vertices_per_line = (number_of_vertices_per_line - 5) / lod_increment + 1;
            int number_main_vertices = number_main_vertices_per_line * number_main_vertices_per_line;

            vertices = new Vector3[number_mesh_edge_vertices + number_edge_connection_vertices + number_main_vertices];
            uvs = new Vector2[vertices.Length];

            int number_mesh_edge_triangles = 8 * (number_of_vertices_per_line - 4);
            int number_main_triangles = (number_main_vertices_per_line - 1) * (number_main_vertices_per_line - 1) * 2;
            triangles = new int[(number_mesh_edge_triangles + number_main_triangles) * 3];

            out_of_mesh_vertices = new Vector3[number_of_vertices_per_line * 4 - 4];
            out_of_mesh_triangles = new int[24 * (number_of_vertices_per_line - 2)];
        }

        public void AddVertex(Vector3 vertex_position, Vector2 uv, int vertex_index)
        {
            if (vertex_index < 0)
            {
                out_of_mesh_vertices[-vertex_index - 1] = vertex_position;
            }
            else
            {
                vertices[vertex_index] = vertex_position;
                uvs[vertex_index] = uv;
            }
        }

        public void AddTriangle(int a, int b, int c)
        {
            if (a < 0 || b < 0 || c < 0)
            {
                out_of_mesh_triangles[out_of_mesh_triangle_index] = a;
                out_of_mesh_triangles[out_of_mesh_triangle_index + 1] = b;
                out_of_mesh_triangles[out_of_mesh_triangle_index + 2] = c;
                out_of_mesh_triangle_index += 3;
            }
            else
            {
                triangles[triangles_index] = a;
                triangles[triangles_index + 1] = b;
                triangles[triangles_index + 2] = c;
                triangles_index += 3;
            }
               
        }
        Vector3[] CalculateNormals()
        {

            Vector3[] vertex_normals = new Vector3[vertices.Length];
            int triangle_count = triangles.Length / 3;
            for (int i = 0; i < triangle_count; i++)
            {
                int normal_triangle_index = i * 3;
                int vertex_index_a = triangles[normal_triangle_index];
                int vertex_index_b = triangles[normal_triangle_index + 1];
                int vertex_index_c = triangles[normal_triangle_index + 2];

                Vector3 triangle_normal = SurfaceNormalFromIndices(vertex_index_a, vertex_index_b, vertex_index_c);
                vertex_normals[vertex_index_a] += triangle_normal;
                vertex_normals[vertex_index_b] += triangle_normal;
                vertex_normals[vertex_index_c] += triangle_normal;
            }

            int border_triangle_count = out_of_mesh_triangles.Length / 3;
            for (int i = 0; i < border_triangle_count; i++)
            {
                int normal_triangle_index = i * 3;
                int vertex_index_a = out_of_mesh_triangles[normal_triangle_index];
                int vertex_index_b = out_of_mesh_triangles[normal_triangle_index + 1];
                int vertex_index_c = out_of_mesh_triangles[normal_triangle_index + 2];

                Vector3 triangle_normal = SurfaceNormalFromIndices(vertex_index_a, vertex_index_b, vertex_index_c);
                if (vertex_index_a >= 0)
                {
                    vertex_normals[vertex_index_a] += triangle_normal;
                }
                if (vertex_index_b >= 0)
                {
                    vertex_normals[vertex_index_b] += triangle_normal;
                }
                if (vertex_index_c >= 0)
                {
                    vertex_normals[vertex_index_c] += triangle_normal;
                }
            }


            for (int index = 0; index < vertex_normals.Length; index++)
            {
                vertex_normals[index].Normalize();
            }

            return vertex_normals;

        }

        Vector3 SurfaceNormalFromIndices(int index_a, int index_b, int index_c)
        {
            Vector3 point_a = (index_a < 0) ? out_of_mesh_vertices[-index_a - 1] : vertices[index_a];
            Vector3 point_b = (index_b < 0) ? out_of_mesh_vertices[-index_b - 1] : vertices[index_b];
            Vector3 point_c = (index_c < 0) ? out_of_mesh_vertices[-index_c - 1] : vertices[index_c];

            Vector3 side_ab = point_b - point_a;
            Vector3 side_ac = point_c - point_a;
            return Vector3.Cross(side_ab, side_ac).normalized;
        }

        public void ProcessMesh()
        {
            if (use_flatshading)
            {
                FlatShading();
            }
            else
            {
                BakeNormals();
            }
        }

        void BakeNormals()
        {
            baked_normals = CalculateNormals();
        }

        void FlatShading()
        {
            Vector3[] flatshaded_vertices = new Vector3[triangles.Length];
            Vector2[] flatshaded_uvs = new Vector2[triangles.Length];

            for (int i = 0; i < triangles.Length; i++)
            {
                flatshaded_vertices[i] = vertices[triangles[i]];
                flatshaded_uvs[i] = uvs[triangles[i]];
                triangles[i] = i;
            }

            vertices = flatshaded_vertices;
            uvs = flatshaded_uvs;
        }


        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

            if (use_flatshading)
            {
                mesh.RecalculateNormals();
            }
            else
            {
                mesh.normals = baked_normals;
            }
            
            return mesh;
        }

    }

}