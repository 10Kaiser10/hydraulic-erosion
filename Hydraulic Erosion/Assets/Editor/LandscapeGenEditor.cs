using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MultiTerrain))]
public class LandscapeGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MultiTerrain mc = (MultiTerrain)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Map"))
        {
            mc.DestroyAll();
            mc.GenerateLandscape();
        }
    }
}
