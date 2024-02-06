using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class TerrainChunck
{
    GameObject meshObject;
    Vector2 position;
    Bounds bounds;
    Vector2 playerPositionReference;
    float maxViewRange;
    
    public TerrainChunck(Vector2 coord, int size, Vector2 playerPosition, float playerTerrainViewRange, Transform parent)
    {
        playerPositionReference = playerPosition;
        maxViewRange = playerTerrainViewRange;
        position = coord * size;
        Vector3 positionIn3D = new Vector3(position.x, 0.0f, position.y);
        bounds = new Bounds(position, Vector2.one * size);

        meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        meshObject.transform.position = positionIn3D;
        meshObject.transform.localScale = Vector3.one * size / 10f;
        meshObject.transform.parent = parent;
        SetVisible(false);
    }

    public void UpdateTerrainChunck()
    {
        float distancePlayerToNearestChunckEdge = Mathf.Sqrt(bounds.SqrDistance(playerPositionReference));
        bool visible = distancePlayerToNearestChunckEdge <= maxViewRange;
        SetVisible(visible);
    }

    public void SetVisible(bool visible)
    {
        meshObject.SetActive(visible);
    }

    public bool IsVisible() => meshObject.activeSelf;
}

