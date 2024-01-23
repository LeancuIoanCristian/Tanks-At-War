using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public static class MeshGenerator 
{
    public static MeshData CreateMeshTerrain(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int indexHeigth = 0; indexHeigth < height; indexHeigth++)
        {
            for (int indexWidth = 0; indexWidth < width; indexWidth++)
            {
                meshData.GetVerticesArray()[vertexIndex] = new Vector3(topLeftX +indexWidth, noiseMap[indexWidth, indexHeigth], topLeftZ - indexHeigth);
                meshData.GetUVsArray()[vertexIndex] = new Vector2(indexWidth / (float)width, indexHeigth / (float)height);

                if (indexWidth < width - 1 && indexHeigth < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }

       
}

public class MeshData
{
    private Vector3[] verticesArray;
    private int[] trianglesArray;
    int triangleIndex;

    private Vector2[] uvs;

    public MeshData(int width, int height)
    {
        verticesArray = new Vector3[width * height];
        uvs = new Vector2[width * height];
        trianglesArray = new int[(width - 1) * (height - 1) * 6];
    }
    public Vector3[] GetVerticesArray() => verticesArray;
    public int[] GetTrianglesArray() => trianglesArray;
    public Vector2[] GetUVsArray() => uvs;
    public void AddTriangle(int pointA, int pointB, int pointC)
    {
        trianglesArray[triangleIndex] = pointA;
        trianglesArray[triangleIndex + 1] = pointB;
        trianglesArray[triangleIndex + 2] = pointC;

        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticesArray;
        mesh.triangles = trianglesArray;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}