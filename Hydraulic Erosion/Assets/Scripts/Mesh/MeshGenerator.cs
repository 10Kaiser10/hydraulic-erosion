using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static Mesh GenerateMesh(float[,] noiseMap, float heightMult, int size, Vector2 offset)
    {
        int sizeX = noiseMap.GetLength(0);
        int sizeY = noiseMap.GetLength(1);

        Vector3[] vertices = new Vector3[sizeX * sizeY];
        int[] triangles = new int[(sizeX - 1) * (sizeY - 1) * 6];
        Vector2[] uvs = new Vector2[sizeX * sizeY];

        int vertIndex = 0;
        for(int i=0;i<sizeX;i++)
        {
            for(int j=0;j<sizeY;j++)
            {
                vertices[i * sizeY + j] = new Vector3(i, heightMult*noiseMap[i, j], j);
                uvs[i * sizeY + j] = new Vector2((i + offset.x) / ((float)sizeX*size), (j + offset.y) / ((float)sizeY*size));

                if(i<sizeX-1 && j<sizeY-1)
                {
                    triangles[vertIndex] = i * sizeY + j;
                    triangles[vertIndex+1] = (i+1) * sizeY + j+1;
                    triangles[vertIndex+2] = (i+1) * sizeY + j;

                    triangles[vertIndex+3] = i * sizeY + j+1;
                    triangles[vertIndex+4] = (i + 1) * sizeY + j + 1;
                    triangles[vertIndex + 5] = i * sizeY + j;

                    vertIndex += 6;
                }
            }
        }

        Mesh terrainMesh = new Mesh();
        terrainMesh.vertices = vertices;
        terrainMesh.triangles = triangles;
        terrainMesh.uv = uvs;
        terrainMesh.RecalculateNormals();
        terrainMesh.RecalculateTangents();

        return terrainMesh;
    }
}
