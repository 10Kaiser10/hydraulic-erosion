using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseMap
{
    public static float[,] NoiseMapGenerator(int sizeX, int sizeY, float scale, int octaves, float persis, float lac, int seed, Vector2 offset)
    {
        float[,] map = new float[sizeX, sizeY];

        float[,] RandOff = new float[octaves, 2];

        System.Random prng = new System.Random(seed);
        for (int i = 0; i < octaves; i++)
        {
            RandOff[i, 0] = prng.Next(-100000, 100000);
            RandOff[i, 1] = prng.Next(-100000, 100000);
        }

        float maxVal = 0;
        float minVal = 1;

        float perlinVal;
        float frequency;
        float amplitude;
        for (int y=0;y<sizeY;y++)
        {
            for(int x=0;x<sizeX;x++)
            {
                frequency = 1;
                amplitude = 1;
                for(int o=0;o<octaves;o++)
                {
                    float sampleX = ((x / scale) + offset.x + RandOff[o, 0])*frequency;
                    float sampleY = ((y / scale) + offset.y + RandOff[o, 1])*frequency;

                    perlinVal = amplitude*(2*Mathf.PerlinNoise(sampleX, sampleY) - 1);
                    map[x, y] += perlinVal;

                    frequency *= lac;
                    amplitude *= persis;
                }
                if(map[x,y]>maxVal)
                {
                    maxVal = map[x, y];
                }
                if (map[x, y] < minVal)
                {
                    minVal = map[x, y];
                }
            }
        }

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                map[x, y] = Mathf.InverseLerp(minVal, maxVal, map[x, y]);
            }
        }

        return map;
    }
}
