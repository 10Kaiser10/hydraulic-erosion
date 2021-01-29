using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    [HideInInspector]
    public float height;

    public MeshFilter meshFilter;
    public MeshRenderer meshRend;

    public void CreateMesh(float[,] noiseMap, int size, Vector2 offset)
    {
        Mesh terrainMesh = MeshGenerator.GenerateMesh(noiseMap, height, size, offset);

        meshFilter.sharedMesh = terrainMesh;
    }
}
