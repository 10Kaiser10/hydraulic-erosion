using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerraintTextureGen))]
public class MeshGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerraintTextureGen mc = (TerraintTextureGen)target;

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