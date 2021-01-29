using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreatorGPU : MonoBehaviour
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

    [Range(0, 300000)]
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

    [HideInInspector]
    public float[,] noiseMap;

    public ComputeShader NoiseMapShader;
    public ComputeShader ErosionShader;

    public MeshCreator meshCreator;

    public void GenerateMap()
    {
        if (scale <= 0){scale = 0.00001f;}
        if (MapWidth <= 0){MapWidth = 1;}
        if (MapHeight <= 0){MapHeight = 1;}
        if (elevation <= 0) { elevation = 0; }
        if (lacunarity < 1){lacunarity = 1;}
        if(brushSize < 1){brushSize = 1;}
        if(startSpeed < 0){startSpeed = 0;}
        if(accel < 0){accel = 0;}
        if(drag < 0){drag = 0;}
        if (startWater < 0) { startWater = 0; }
        if (sedimentCapaFactor < 0) { sedimentCapaFactor = 0; }
        if (depSpeed < 0) { depSpeed = 0; }
        if (eroSpeed < 0) { eroSpeed = 0; }
        if (gravity < 0) { gravity = 0; }
        if (evaporateSpeed < 0) { evaporateSpeed = 0; }


        noiseMap = NoiseMapGPU.NoiseMapGenerator(MapHeight, MapWidth, scale, octaves, persistance, lacunarity, seed, offset, NoiseMapShader);

        noiseMap = NoiseMapGPU.erosion(noiseMap, numIter, MapHeight, MapWidth, lifetime, brushSize, startSpeed, accel, drag, seed, ErosionShader, startWater, sedimentCapaFactor, depSpeed, eroSpeed, gravity, evaporateSpeed);

        gameObject.GetComponent<DrawNoise>().DrawNoiseTex(noiseMap);

        meshCreator.height = elevation;
        meshCreator.CreateMesh(noiseMap, 1, new Vector2(0,0));
    }
}
