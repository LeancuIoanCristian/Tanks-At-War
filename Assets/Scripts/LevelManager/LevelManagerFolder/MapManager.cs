using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

class MapManager: MonoBehaviour
{
    [SerializeField] private bool autoGenerate = false;
    [SerializeField] private MapGenerator mapGeneratorReference;

    [SerializeField] private List<GameObject> objects;
    [SerializeField] private int maxObjects;
    [SerializeField] private int width, height;
    [SerializeField] private List<GeneratedSpaces> spacesList = new List<GeneratedSpaces>();
    [SerializeField] private int spacingSize;

    private static MapManager instance = null;
    public static MapManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MapManager();
            }
            return instance;
        }
    }

    public void Awake()
    {
        instance = this;
    }

    public bool GetAuto() => autoGenerate;
    public void SetUpLevel()
    {

    }
}

[System.Serializable]
public struct GeneratedSpaces
{
    private bool ocupied;
    private int xPosition;
    private int zPosition;

    public bool GetSpaceState() => ocupied;
    public void SpaceInitialize(int x_value, int z_value, bool value) { ocupied = value; SetCoordonates(x_value, z_value); }
    public void ChangeState(bool value) => ocupied = value;

    public int GetX() => xPosition;
    public int GetZ() => zPosition;
    public void SetCoordonates(int x_value, int z_value) { xPosition = x_value; zPosition = z_value; }
}

