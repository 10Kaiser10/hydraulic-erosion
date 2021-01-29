using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainTextureGen))]
public class MeshGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainTextureGen mc = (TerrainTextureGen)target;

        if (DrawDefaultInspector())
        {
            mc.GenerateTexture();
        }

        if (GUILayout.Button("Generate Texture"))
        {
            mc.GenerateTexture();
        }
    }
}