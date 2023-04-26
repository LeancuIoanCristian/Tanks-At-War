using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Terrain_Generation
{

    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MapGenerator map_generator = (MapGenerator)target;


            if (DrawDefaultInspector())
            {
                if (map_generator.auto_update)
                {
                    map_generator.GenerateMapData();
                }
            }


            if (GUILayout.Button("Generate"))
            {
                map_generator.GenerateMapData();
            }
        }
    }

}
