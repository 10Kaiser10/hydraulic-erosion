using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTerrain : MonoBehaviour
{
    public int MapHeight;
    public int MapWidth;
    public float elevation;
    public float scale;

    [Range(1, 10)]
    public int octaves;
    public float lacunarity;
    [Range(0, 1)]
    public float persistance;
    public int seed;
    public Vector2 offset;

    [Range(0, 1000000)]
    public int numIter;
    [Range(0, 300)]
    public int lifetime;
    public float accel;
    public float drag;
    [Range(1, 5)]
    public int brushSize;
    public float startSpeed;
    public float startWater;
    public float sedimentCapaFactor;
    public float depSpeed;
    public float eroSpeed;
    public float gravity;
    public float evaporateSpeed;

    public ComputeShader NoiseMapShader;
    public ComputeShader ErosionShader;

    public Material terrainMaterial;

    public float[,] map;

    GameObject[] objects;

    [Range(1,10)]
    public int size = 2;

    public void DestroyAll()
    {
        if(objects == null) { return; }
        foreach(GameObject obj in objects)
        {
            if(obj != null)
            {
                DestroyImmediate(obj);
            }
        }
    }

    public void GenerateLandscape()
    {
        objects = new GameObject[size * size];

        if (scale <= 0) { scale = 0.00001f; }
        if (MapWidth <= 0) { MapWidth = 1; }
        if (MapHeight <= 0) { MapHeight = 1; }
        if (elevation <= 0) { elevation = 1;}
        if (lacunarity < 1) { lacunarity = 1; }
        if (brushSize < 1) { brushSize = 1; }
        if (startSpeed < 0) { startSpeed = 0; }
        if (accel < 0) { accel = 0; }
        if (drag < 0) { drag = 0; }
        if (startWater < 0) { startWater = 0; }
        if (sedimentCapaFactor < 0) { sedimentCapaFactor = 0; }
        if (depSpeed < 0) { depSpeed = 0; }
        if (eroSpeed < 0) { eroSpeed = 0; }
        if (gravity < 0) { gravity = 0; }
        if (evaporateSpeed < 0) { evaporateSpeed = 0; }

        Vector2 thisOffset = offset;
        map = NoiseMapGPU.NoiseMapGenerator(MapWidth * size, MapHeight * size, scale, octaves, persistance, lacunarity, seed, thisOffset, NoiseMapShader);
        map = NoiseMapGPU.erosion(map, numIter*size*size, MapWidth * size, MapHeight * size, lifetime, brushSize, startSpeed, accel, drag, seed, ErosionShader, startWater, sedimentCapaFactor, depSpeed, eroSpeed, gravity, evaporateSpeed);
        gameObject.GetComponent<DrawNoise>().DrawNoiseTex(map);
        for (int i=0;i<size;i++)
        {
            for(int j=0;j<size;j++)
            {
                thisOffset.x = offset.x + i * MapWidth;
                thisOffset.y = offset.y + j * MapHeight;

                float[,] chunk = new float[MapWidth, MapHeight];

                for (int x = 0; x < MapWidth; x++)
                {
                    for (int y = 0; y < MapHeight; y++)
                    {
                        chunk[x, y] = map[i * MapWidth + x, j * MapHeight + y];
                    }
                }

                objects[i * size + j] = new GameObject();
                GameObject obj = objects[i * size + j];
                obj.transform.position = new Vector3(i * MapWidth * 10 - 10*i,0, j * MapHeight * 10 - 10*j);
                obj.transform.localScale = new Vector3(10, 1, 10);
                MeshFilter mf = obj.AddComponent<MeshFilter>();
                MeshRenderer mr = obj.AddComponent<MeshRenderer>();
                mr.sharedMaterial = terrainMaterial;
                MeshCreator mc = obj.AddComponent<MeshCreator>();
                mc.height = elevation;
                mc.meshFilter = mf;
                mc.meshRend = mr;
                mc.CreateMesh(chunk, size, thisOffset);
                
            }
        }
    }
}
