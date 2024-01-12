using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LevelManager.TerrainGeneration
{
    [CustomEditor(typeof(MapGenerator))]
    class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MapGenerator mapGenerator = (MapGenerator)target;

            DrawDefaultInspector();
            if (GUILayout.Button("Generate"))
            {
                mapGenerator.GeneratePerlinMap();
            }
        }
    }
}
