using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Terrain_Generation
{

	[CustomEditor(typeof(PreviewMap))]
	public class MapPreviewEditor : Editor
	{

		public override void OnInspectorGUI()
		{
			PreviewMap map_preview = (PreviewMap)target;

			if (DrawDefaultInspector())
			{
				if (map_preview.auto_update)
				{
					map_preview.DrawMapInEditor();
				}
			}

			if (GUILayout.Button("Generate"))
			{
				map_preview.DrawMapInEditor();
			}
		}
	}
}
