using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] height_map, float height_multiplier, AnimationCurve height_curve, int level_of_detail)
    {
        int width = height_map.GetLength(0);
        int height = height_map.GetLength(1);
        float top_left_width = (width - 1) / 2f;
        float top_left_height = (height - 1) / 2f;
        int increment_of_detail = (level_of_detail == 0)?1:2 * level_of_detail;

        int verteces_per_line = (width - 1) / increment_of_detail + 1;

        MeshData mesh_data = new MeshData(verteces_per_line, verteces_per_line);
        int vertex_index = 0;

        for (int height_index = 0; height_index < height; height_index += increment_of_detail)
        {
            for (int width_index = 0; width_index < width; width_index += increment_of_detail)
            {
                mesh_data.GetVertices()[vertex_index] = new Vector3(top_left_width + width_index, height_curve.Evaluate(height_map[width_index, height_index]) * height_multiplier, top_left_height - height_index);
                mesh_data.GetUVs()[vertex_index] = new Vector2(width_index / (float)width, height_index / (float)height);
                if (width_index < width - 1 && height_index < height - 1)
                {
                    mesh_data.AddTriangle(vertex_index, vertex_index + verteces_per_line + 1, vertex_index + verteces_per_line);
                    mesh_data.AddTriangle(vertex_index + verteces_per_line + 1, vertex_index, vertex_index +1);
                }

                vertex_index++;
            }
        }

        return mesh_data;
    }
}

public class MeshData
{
    [SerializeField] private Vector3[] vertices;
    [SerializeField] private Vector2[] uvs;
    [SerializeField] private int[] triangles;
    [SerializeField] private int triangles_index;

    public Vector3[] GetVertices() => vertices;
    public Vector2[] GetUVs() => uvs;
    public int[] GetTriangle() => triangles;

    public MeshData(int mesh_width, int mesh_height)
    {
        vertices = new Vector3[mesh_width * mesh_height];
        uvs = new Vector2[mesh_width * mesh_height];
        triangles = new int[(mesh_width - 1) * (mesh_height - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangles_index] = a;
        triangles[triangles_index + 1] = b;
        triangles[triangles_index + 2] = c;
        triangles_index += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }

}