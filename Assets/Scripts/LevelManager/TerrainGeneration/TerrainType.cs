using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct TerrainType
{
    [SerializeField] private string name;
    [SerializeField] private float height;
    [SerializeField] private Color color;

    public float GetHeight() => height;
    public Color GetColor() => color;
}

