using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureGen : MonoBehaviour
{
    public MultiTerrain mapCreator;

    public Material terrainMat;
    public Color grass;
    public Color rock;
    public Color snow;

    public float grassRockLim;
    public float grassRockBlend;
    public float snowLim;
    public float snowBlend;

    int sizeX, sizeY;
    float[,] map;

    Vector3 heightAndGrad(int x, int y)
    {
        Vector3 hng = new Vector3();

        x = Mathf.Min(x, sizeX - 2);
        y = Mathf.Min(y, sizeY - 2);

        float a = map[x, y];
        float b = map[x + 1, y];
        float c = map[x, y + 1];
        float d = map[x + 1, y + 1];

        hng.z = (a + b + c + d) / 4;
        hng.x = (b - a + d - c) / 2;
        hng.y = (c - a + d - b) / 2;

        return hng;
    }

    public void GenerateTexture()
    {
        map = mapCreator.map;

        sizeX = map.GetLength(0);
        sizeY = map.GetLength(1);

        Color[,] colorMap = new Color[sizeX, sizeY];

        for(int i=0; i<sizeX; i++)
        {
            for (int j=0; j < sizeY; j++)
            {
                Vector3 gradNHeight = heightAndGrad(i, j);
                float grad = Mathf.Sqrt(gradNHeight.x * gradNHeight.x + gradNHeight.y * gradNHeight.y);

                if(grad < grassRockLim - grassRockBlend)
                {
                    colorMap[i, j] = grass;
                }
                else if(grad > grassRockLim + grassRockBlend)
                {
                    colorMap[i, j] = rock;
                }
                else
                {
                    Color col = Color.Lerp(grass, rock, (grad - (grassRockLim - grassRockBlend)) / (2 * grassRockBlend));
                    colorMap[i, j] = col;
                }
            }
        }
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                float height = heightAndGrad(i, j).z;

                if (height < snowLim - snowBlend)
                {
                    colorMap[i, j] = colorMap[i, j];
                }
                else if (height > snowLim + snowBlend)
                {
                    colorMap[i, j] = snow;
                }
                else
                {
                    Color col = Color.Lerp(colorMap[i, j], snow, (height - (snowLim - snowBlend)) / (2 * snowBlend));
                    colorMap[i, j] = col;
                }
            }
        }

        Texture2D texture = new Texture2D(sizeX, sizeY);

        for (int i=0; i<sizeX; i++)
        {
            for(int j=0; j<sizeY; j++)
            {
                texture.SetPixel(i, j, colorMap[i, j]);                
            }
        }

        texture.Apply();

        terrainMat.SetTexture("_MainTex", texture);
    }
}
