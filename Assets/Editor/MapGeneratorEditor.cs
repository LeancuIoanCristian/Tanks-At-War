using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
                map_generator.GenerateMap();
            }
        }


        if (GUILayout.Button("Generate"))
        {
            map_generator.GenerateMap();
        }
    }
}
