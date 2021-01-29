using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNoise : MonoBehaviour
{
    public Renderer rend;

    public void DrawNoiseTex(float[,] noiseArr)
    {
        int height = noiseArr.GetLength(0);
        int width = noiseArr.GetLength(1);

        Texture2D texture = new Texture2D(height, width);
        Color[] colorArr = new Color[height*width]; 

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                colorArr[i + j*height] = Color.Lerp(Color.black, Color.white, noiseArr[i, j]);
            }
        }

        texture.SetPixels(colorArr);
        texture.Apply();

        rend.sharedMaterial.mainTexture = texture;
        rend.transform.localScale = new Vector3(height, 1, width);
    }
}
