using System;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Bezier))]
public class BezierHandler : Editor
{
    private void OnSceneGUI()
    {
        Handles.DrawBezier(new Vector3(1f,1f,1f),Vector3.zero, new Vector3(2,2,1), new Vector3(3,5,1),Color.black,null,2f);
    }
}
