//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(MeshCreator))]
//public class MeshGenEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        MeshCreator mc = (MeshCreator)target;

//        if (DrawDefaultInspector())
//        {
//            mc.CreateMesh();
//        }

//        if (GUILayout.Button("Generate Map"))
//        {
//            mc.CreateMesh();
//        }
//    }
//}
