using UnityEngine;
using System.Collections;


namespace Terrain_Generation
{
    class PreviewMap : MonoBehaviour
    {
		public Renderer texture_render;
		public MeshFilter mesh_filter;
		public MeshRenderer mesh_renderer;


		public enum DrawMode { NoiseMap, Mesh, FalloffMap };
		public DrawMode draw_mode;

		public MeshSettings mesh_settings;
		public HeightMapSettings height_map_settings;
		public TextureData texture_data;

		public Material terrain_material;



		[Range(0, MeshSettings.munber_of_supported_level_of_details - 1)]
		public int editor_preview_LOD;
		public bool auto_update;




		public void DrawMapInEditor()
		{
			texture_data.ApplyToMaterial(terrain_material);
			texture_data.UpdateMeshHeights(terrain_material, height_map_settings.minHeight, height_map_settings.maxHeight);
			HeightMap height_map = HeightMapGenerator.GenerateHeightMap(mesh_settings.number_of_vertices_per_line, mesh_settings.number_of_vertices_per_line, height_map_settings, Vector2.zero);

			if (draw_mode == DrawMode.NoiseMap)
			{
				DrawTexture(TextureGenerator.DrawTextureFromHightMap(height_map));
			}
			else if (draw_mode == DrawMode.Mesh)
			{
				DrawMesh(MeshGenerator.GenerateTerrainMesh(height_map.values, mesh_settings, editor_preview_LOD));
			}
			else if (draw_mode == DrawMode.FalloffMap)
			{
				DrawTexture(TextureGenerator.DrawTextureFromHightMap(new HeightMap(FalloffGenerator.GenerateFalloffMap(mesh_settings.number_of_vertices_per_line), 0, 1)));
			}
		}





		public void DrawTexture(Texture2D texture)
		{
			texture_render.sharedMaterial.mainTexture = texture;
			texture_render.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10f;

			texture_render.gameObject.SetActive(true);
			mesh_filter.gameObject.SetActive(false);
		}

		public void DrawMesh(MeshData meshData)
		{
			mesh_filter.sharedMesh = meshData.CreateMesh();

			texture_render.gameObject.SetActive(false);
			mesh_filter.gameObject.SetActive(true);
		}



		void OnValuesUpdated()
		{
			if (!Application.isPlaying)
			{
				DrawMapInEditor();
			}
		}

		void OnTextureValuesUpdated()
		{
			texture_data.ApplyToMaterial(terrain_material);
		}

		void OnValidate()
		{

			if (mesh_settings != null)
			{
				mesh_settings.OnValuesUpdated -= OnValuesUpdated;
				mesh_settings.OnValuesUpdated += OnValuesUpdated;
			}
			if (height_map_settings != null)
			{
				height_map_settings.OnValuesUpdated -= OnValuesUpdated;
				height_map_settings.OnValuesUpdated += OnValuesUpdated;
			}
			if (texture_data != null)
			{
				texture_data.OnValuesUpdated -= OnTextureValuesUpdated;
				texture_data.OnValuesUpdated += OnTextureValuesUpdated;
			}

		}
	}
}
