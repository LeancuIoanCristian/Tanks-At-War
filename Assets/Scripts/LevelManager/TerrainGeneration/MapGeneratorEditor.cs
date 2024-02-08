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
            //if (mapGenerator.GetAutoErodeValue())
            //{
            //    AutoErode(mapGenerator);
            //}

        }

        

        if (GUILayout.Button("Generate"))
        {
            mapGenerator.GenerateMap();
        }

        if (GUILayout.Button("Erode"))
        {
            mapGenerator.Erode();
        }

       
    }

    private void AutoErode(MapGenerator mapGenerator)
    { 
        new WaitForSeconds(15);
        mapGenerator.Erode();
        AutoErode(mapGenerator);
    }
}

