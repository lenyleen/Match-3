using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameButton),true)]
public class CustomButtonInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        GameButton targetGameButton = (GameButton)target;
        EditorGUILayout.LabelField("ButtonType:");
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}