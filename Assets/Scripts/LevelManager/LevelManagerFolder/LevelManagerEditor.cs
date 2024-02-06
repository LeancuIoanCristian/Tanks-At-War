using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
class LevelManagerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        MapManager levelManager = (MapManager)target;

        if (DrawDefaultInspector())
        {
            if (levelManager.GetAuto())
            {
                levelManager.SetUpLevel();
            }
        }

        if (GUILayout.Button("Generate Level"))
        {
            levelManager.SetUpLevel();
        }
    }
    
}

