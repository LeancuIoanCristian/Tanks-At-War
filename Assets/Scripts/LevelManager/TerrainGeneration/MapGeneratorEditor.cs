using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MapGenerator))]
class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGenerator.GetAutoRegenerateValue()) 
            {
                mapGenerator.GenerateMap();
            }

        }

        if (GUILayout.Button("Generate"))
        {
            mapGenerator.GenerateMap();
        }
    }
}

