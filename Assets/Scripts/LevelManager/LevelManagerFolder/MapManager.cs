using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

class MapManager: MonoBehaviour
{
    [SerializeField] private bool autoGenerate = false;
    [SerializeField] private MapGenerator mapGeneratorReference;

    [SerializeField] private List<GameObject> possibleObjects;
    [SerializeField] private List<GameObject> spawnedObjects;
    [SerializeField] private int maxObjects;
    [SerializeField] private int width, height;
    [SerializeField] private List<GeneratedSpaces> spacesList = new List<GeneratedSpaces>();
    [SerializeField] private int spacingSizeX;
    [SerializeField] private int spacingSizeY;

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

    private void Start()
    {
        SetUpLevel();
    }
    public void SetUpLevel()
    {
        mapGeneratorReference.GenerateMap();
        width = mapGeneratorReference.GetWidth();
        height = mapGeneratorReference.GetHeight();
        spacingSizeX = width / (int)Mathf.Sqrt(maxObjects);
        spacingSizeY = height / (int)Mathf.Sqrt(maxObjects);
        spacesList.Clear();
        UpdateListSpawnedObjects();
        for (int indexHeight = 0; indexHeight < height/ spacingSizeY; indexHeight ++)
        {
            for (int indexWidth = 0; indexWidth < width/ spacingSizeX; indexWidth ++ )
            {
                GeneratedSpaces newSpace = new GeneratedSpaces();
                newSpace.SpaceInitialize(indexWidth * spacingSizeX, indexHeight * spacingSizeY, false);
                spacesList.Add(newSpace);
            }
        }

        for (int index = 0; index < spacesList.Count - 1; index++)
        {
            int xposToMap = Random.Range(spacesList[index].GetX(), spacesList[index].GetX() + spacingSizeX - 1);
            int zposToMap = Random.Range(spacesList[index].GetZ(), spacesList[index].GetZ() + spacingSizeY - 1);
            int xpos = (xposToMap - ((width -1) /2)) * (width / spacingSizeX);
            int zpos = (zposToMap - ((height - 1) / 2)) * (height / spacingSizeY);


            if (mapGeneratorReference.GetHeightMap(xposToMap, zposToMap) >= 0.5f && spacesList[index].GetSpaceState() == false)
            {
                float ypos = mapGeneratorReference.GetHeightMap(xposToMap, zposToMap) * mapGeneratorReference.GetHeightMultiplier() * mapGeneratorReference.GetAnimationCurve().Evaluate(mapGeneratorReference.GetHeightMap(xposToMap, zposToMap));
                Vector3 positionObject = new Vector3(xpos, ypos, zpos);
                int objNumber = Random.Range(0, possibleObjects.Count);
                GameObject newObject = Instantiate(possibleObjects[objNumber]);
                if(newObject.GetComponent<NavMeshAgent>())
                {
                    newObject.GetComponent<NavMeshAgent>().Warp(positionObject);
                }
                else
                {
                    newObject.transform.position = positionObject;
                }
                spawnedObjects.Add(newObject);
                spacesList[index].ChangeState(true);
            }


        }

    }
    //mapGeneratorReference.GetHeightMap(xpos, zpos) * mapGeneratorReference.GetHeightMultiplier() * mapGeneratorReference.GetAnimationCurve().Evaluate(mapGeneratorReference.GetHeightMap(xpos, zpos))
    public void UpdateListSpawnedObjects()
    {
        for (int index = 0; index < spawnedObjects.Count; index++)
        {
            GameObject.DestroyImmediate(spawnedObjects[index].gameObject);

        }
        spawnedObjects.Clear();
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

