using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

class InfiniteTerrain : MonoBehaviour
{
    [SerializeField] private Dictionary<Vector2, TerrainChunck> terrainChunckDictionary = new Dictionary<Vector2, TerrainChunck>();
    [SerializeField] private List<TerrainChunck> terrainChuncksList = new List<TerrainChunck>();

    private const float maxViewRange = 450f;
    [SerializeField] private Transform playerPositionReference;

    public static Vector2 playerPosition;
    private int chunkSize;
    private int chunkVisibleInDistance;
    [SerializeField] private Transform parentReference;

    private void Start()
    {
        chunkSize = EndlessGenerator.chunkSize - 1;
        chunkVisibleInDistance = Mathf.RoundToInt(maxViewRange / chunkSize);
    }
    private void Update()
    {
        playerPosition = new Vector2(playerPositionReference.position.x, playerPositionReference.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        for (int index = 0; index < terrainChuncksList.Count; index++)
        {
            terrainChuncksList[index].SetVisible(false);
        }

        terrainChuncksList.Clear();

        int currentChunckCoordX = Mathf.RoundToInt(playerPosition.x/ chunkSize);
        int currentChunckCoordY = Mathf.RoundToInt(playerPosition.y / chunkSize);

        for (int yIndexOffset = -chunkVisibleInDistance; yIndexOffset <= chunkVisibleInDistance; yIndexOffset++)
        {
            for (int xIndexOffset = -chunkVisibleInDistance; xIndexOffset <= chunkVisibleInDistance; xIndexOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunckCoordX + xIndexOffset, currentChunckCoordY + yIndexOffset);
                if (terrainChunckDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunckDictionary[viewedChunkCoord].UpdateTerrainChunck();
                    if (terrainChunckDictionary[viewedChunkCoord].IsVisible())
                    {
                        terrainChuncksList.Add(terrainChunckDictionary[viewedChunkCoord]);
                    }
                    
                }
                else
                {
                    terrainChunckDictionary.Add(viewedChunkCoord, new TerrainChunck(viewedChunkCoord, chunkSize, playerPosition, maxViewRange, parentReference));
                    
                }

            }
        }

       
    }
}

