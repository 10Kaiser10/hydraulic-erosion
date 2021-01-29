using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public int MapHeight;
    public int MapWidth;
    public float scale;

    [Range(1,10)]
    public int octaves;
    public float lacunarity;
    [Range(0,1)]
    public float persistance;
    public int seed;
    public Vector2 offset;

    public void GenerateMap()
    {
        if(scale<=0)
        {
            scale = 0.00001f;
        }
        if(MapWidth<=0)
        {
            MapWidth = 1;
        }
        if(MapHeight<=0)
        {
            MapHeight = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }


        float[,] noiseMap = NoiseMap.NoiseMapGenerator(MapHeight, MapWidth, scale, octaves, persistance, lacunarity, seed, offset);

        gameObject.GetComponent<DrawNoise>().DrawNoiseTex(noiseMap);
    }   
}
