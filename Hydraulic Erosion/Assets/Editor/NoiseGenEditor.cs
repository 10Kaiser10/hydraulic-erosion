using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapCreator))]
public class NoiseGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapCreator mc = (MapCreator)target;

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
