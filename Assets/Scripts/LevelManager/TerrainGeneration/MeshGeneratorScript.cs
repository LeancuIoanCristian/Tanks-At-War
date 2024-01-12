using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelManager.TerrainGeneration
{
    public class MeshGenerator : MonoBehaviour
    {
        [SerializeField] private Mesh mesh_reference;
        [SerializeField] private Vertex[] vertices;
        [SerializeField] private Vector3[] normals;
        [SerializeField] private Triangle[] triangles;
        private int x_length;
        private int z_length;

        public void SetSize(int x_value, int z_value)
        {
            x_length = x_value;
            z_length = z_value;
        }

        // Start is called before the first frame update
        void Start()
        {
            mesh_reference = new Mesh();
            if (GetComponent<MeshFilter>() == null)
            {
                this.gameObject.AddComponent<MeshFilter>();
            }
            else
            {
                GetComponent<MeshFilter>().mesh = mesh_reference;
            }


            CreateMeshTerrain();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateMeshTerrain()
        {
            for (int index_x = 0; index_x < x_length - 2; index_x++)
            {
                for (int index_z = 0; index_z < z_length - 2; index_z++)
                {
                    Vertex vertex1 = new Vertex();
                    vertex1.SetVertex(index_x, 0, index_z);
                    vertices[x_length * index_x + index_z] = vertex1;

                }
            }

            for (int indexer = 0; indexer < vertices.Length - 2; indexer += 2)
            {
                Triangle triangle1 = new Triangle();
                triangle1.SetNodes(vertices[indexer], vertices[indexer + 1], vertices[indexer + x_length]);
            }
        }

        public void SetGridSize(int x_value, int z_value)
        {
            x_length = x_value;
            z_length = z_value;
        }
    }

    struct Vertex
    {
        private int x_axis_value;
        private int y_axis_value;
        private int z_axis_value;

        public int GetX() => x_axis_value;
        public int GetY() => y_axis_value;
        public int GetZ() => z_axis_value;

        public void SetX(int value) => x_axis_value = value;
        public void SetY(int value) => y_axis_value = value;
        public void SetZ(int value) => z_axis_value = value;

        public void SetVertex(int x_value, int y_value, int z_value)
        {
            SetX(x_value);
            SetY(y_value);
            SetZ(z_value);
        }
    }

    struct Triangle
    {
        private Vertex node_1;
        private Vertex node_2;
        private Vertex node_3;

        public void SetNodes(Vertex vertex1, Vertex vertex2, Vertex vertex3)
        {
            node_1 = vertex1;
            node_2 = vertex2;
            node_3 = vertex3;
        }
    }
}
