using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoosterImage))]
public class CustomBoosterImageInspector : Editor
{
    private Vector2 scroll;
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        BoosterImage boosterImage = (BoosterImage)target;
        EditorGUILayout.LabelField("Description:");
        GUILayout.Space(-100);
        scroll = EditorGUILayout.BeginScrollView(scroll);
        boosterImage.description = EditorGUILayout.TextField(boosterImage.description,GUILayout.Width(200),GUILayout.Height(100));
        EditorGUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
        if(boosterImage.sprite != null)
            EditorGUILayout.LabelField(new GUIContent(boosterImage.sprite.texture),GUILayout.Width(50),GUILayout.Height(50));
    }   
}