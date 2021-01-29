using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapCreatorGPU))]
public class NoiseGenEditorGPU : Editor
{
    public override void OnInspectorGUI()
    {
        MapCreatorGPU mc = (MapCreatorGPU)target;

        if (DrawDefaultInspector())
        {
            mc.GenerateMap();
        }

        if(GUILayout.Button("Generate Map"))
        {
            mc.GenerateMap();
        }
    }
}
