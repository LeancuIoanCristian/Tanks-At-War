using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Terrain_Generation
{
    public class MapDisplayer : MonoBehaviour
    {
        [SerializeField] private Renderer texture_rendurer;
        [SerializeField] private MeshFilter mesh_filter;
        [SerializeField] private MeshRenderer mesh_renderer;

        public void DrawTextureMap(Texture2D texture)
        {
            texture_rendurer.sharedMaterial.mainTexture = texture;
            texture_rendurer.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }

        public void DrawMeshMap(MeshData mesh_data, Texture2D texture)
        {
            mesh_filter.sharedMesh = mesh_data.CreateMesh();
            mesh_renderer.sharedMaterial.mainTexture = texture;
        }
    }
}


