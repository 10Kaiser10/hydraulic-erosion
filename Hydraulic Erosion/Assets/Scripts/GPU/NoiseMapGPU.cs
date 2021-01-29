using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseMapGPU
{
    public static float[,] NoiseMapGenerator(int sizeX, int sizeY, float scale, int octaves, float persis, float lac, int seed, Vector2 offset, ComputeShader NoiseMapShader)
    {
        float[,] map = new float[sizeX, sizeY];
        float[] mapArr = new float[sizeX * sizeY];

        Vector2[] RandOff = new Vector2[octaves];

        System.Random prng = new System.Random(seed);
        for (int i = 0; i < octaves; i++)
        {
            RandOff[i].x = prng.Next(-100000, 100000);
            RandOff[i].y = prng.Next(-100000, 100000);
        }

        int floatToIntMultiplier = 1000;
        int[] minMaxHeight = { floatToIntMultiplier * octaves, 0 };

        ComputeBuffer rndOffsetsBuffer = new ComputeBuffer(RandOff.Length, 2 * sizeof(float));
        rndOffsetsBuffer.SetData(RandOff);
        NoiseMapShader.SetBuffer(0, "RandOff", rndOffsetsBuffer);

        ComputeBuffer mapBuffer = new ComputeBuffer(mapArr.Length, sizeof(float));
        mapBuffer.SetData(mapArr);
        NoiseMapShader.SetBuffer(0, "map", mapBuffer);

        ComputeBuffer minMaxBuffer = new ComputeBuffer(minMaxHeight.Length, sizeof(int));
        minMaxBuffer.SetData(minMaxHeight);
        NoiseMapShader.SetBuffer(0, "minMax", minMaxBuffer);

        float[] offsets = { offset.x, offset.y };
        NoiseMapShader.SetFloats("offset", offsets);

        NoiseMapShader.SetFloat("lac", lac);
        NoiseMapShader.SetFloat("scale", scale);
        NoiseMapShader.SetFloat("persis", persis);
        NoiseMapShader.SetInt("floatToIntMultiplier", floatToIntMultiplier);
        NoiseMapShader.SetInt("octaves", octaves);
        NoiseMapShader.SetInt("sizeX", sizeX);
        NoiseMapShader.SetInt("sizeY", sizeY);

        NoiseMapShader.Dispatch(0, mapArr.Length/100, 1, 1);

        mapBuffer.GetData(mapArr);
        minMaxBuffer.GetData(minMaxHeight);
        mapBuffer.Release();
        minMaxBuffer.Release();
        rndOffsetsBuffer.Release();

        float minVal = (float)minMaxHeight[0] / (float)floatToIntMultiplier;
        float maxVal = (float)minMaxHeight[1] / (float)floatToIntMultiplier;

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                map[x, y] = Mathf.InverseLerp(minVal, maxVal, mapArr[sizeY*x+ y]);
            }
        }

        return map;
    }


    public static float[,] erosion(float[,] map, int numIter,int sizeX, int sizeY, int lifetime, int brushSize, float startSpeed, float accel, float drag, int seed, ComputeShader ErosionShader, float startWater, float sedimentCapaFactor, float depSpeed, float eroSpeed, float gravity, float evaporateSpeed)
    {
        float[] mapArr = new float[sizeX * sizeY];

        for(int i=0;i<sizeX;i++)
        {
            for(int j=0;j<sizeY;j++)
            {
                mapArr[i * sizeY + j] = map[i, j];
            }
        }

        Vector2[] StartPos = new Vector2[numIter];

        System.Random prng = new System.Random(seed);
        for (int i = 0; i < numIter; i++)
        {
            StartPos[i].x = prng.Next(0, sizeX-2);
            StartPos[i].y = prng.Next(0, sizeY-2);
        }

        //Vector3[] erosionArr = new Vector3[lifetime * numIter];
        //for(int i=0; i<lifetime*numIter;i++)
        //{
        //    erosionArr[i] = Vector3.zero;
        //}

        Vector3[] brush = new Vector3[brushSize * brushSize * 4];
        float normalizeC = 0;
        for(int i=0;i< brushSize;i++)
        {
            for(int j=0;j<brushSize;j++)
            {
                normalizeC += 1.1f * (i + j + 1);
                brush[i * brushSize * 2 + j].z = (i + j + 1);
                brush[(2 * brushSize - 1 - i) * brushSize * 2 + j].z = (i + j + 1);
                brush[(2 * brushSize - 1 - i) * brushSize * 2 + (2 * brushSize - 1 - j)].z = (i + j + 1);
                brush[i * brushSize * 2 + (2 * brushSize - 1 - j)].z = (i + j + 1);
            }
        }
        for (int i = 0; i < brushSize*2; i++)
        {
            for (int j = 0; j < brushSize*2; j++)
            {
                brush[i * brushSize * 2 + j].z /= normalizeC;
                brush[i * brushSize * 2 + j].x = i - (brushSize - 1);
                brush[i * brushSize * 2 + j].y = j - (brushSize - 1);
            }
        }

        ComputeBuffer heightMapBuffer = new ComputeBuffer(mapArr.Length, sizeof(float));
        heightMapBuffer.SetData(mapArr);
        ErosionShader.SetBuffer(0, "map", heightMapBuffer);

        ComputeBuffer startPosBuffer = new ComputeBuffer(numIter, 2*sizeof(float));
        startPosBuffer.SetData(StartPos);
        ErosionShader.SetBuffer(0, "startPos", startPosBuffer);

        //ComputeBuffer changeBuffer = new ComputeBuffer(erosionArr.Length, 3 * sizeof(float));
        //changeBuffer.SetData(erosionArr);
        //ErosionShader.SetBuffer(0, "changes", changeBuffer);

        ComputeBuffer brushBuffer = new ComputeBuffer(brush.Length, 3 * sizeof(float));
        brushBuffer.SetData(brush);
        ErosionShader.SetBuffer(0, "brush", brushBuffer);

        ErosionShader.SetInt("sizeX", sizeX);
        ErosionShader.SetInt("sizeY", sizeY);
        ErosionShader.SetInt("lifetime", lifetime);
        ErosionShader.SetInt("brushSize", brushSize);
        ErosionShader.SetFloat("startSpeed", startSpeed);
        ErosionShader.SetFloat("accel", accel);
        ErosionShader.SetFloat("drag", drag);
        ErosionShader.SetFloat("startWater", startWater);
        ErosionShader.SetFloat("sedimentCapaFactor", sedimentCapaFactor);
        ErosionShader.SetFloat("depSpeed", depSpeed);
        ErosionShader.SetFloat("eroSpeed", eroSpeed);
        ErosionShader.SetFloat("gravity", gravity);
        ErosionShader.SetFloat("evaporateSpeed", evaporateSpeed);

        ErosionShader.Dispatch(0, numIter/1000, 1, 1);

        heightMapBuffer.GetData(mapArr);
        heightMapBuffer.Release();
        startPosBuffer.Release();
        brushBuffer.Release();

        //for debugging the drop path
        //for (int i = 0; i < lifetime; i++)
        //{
        //    int dropId = i;
        //    Vector3 posAmt = erosionArr[dropId];
        //    mapArr[(int)posAmt.x * sizeY + (int)posAmt.y] = 1;
        //}

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                map[i, j] = mapArr[i * sizeY + j]; 
            }
        }

        return map;
    }
}
