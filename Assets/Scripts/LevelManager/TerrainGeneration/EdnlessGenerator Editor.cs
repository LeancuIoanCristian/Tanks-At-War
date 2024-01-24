using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EndlessGenerator))]
class EdnlessGenerator_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        EndlessGenerator mapGenerator = (EndlessGenerator)target;

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

