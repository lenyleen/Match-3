using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StoreItem), true)]
public class StoreItemCustomInspector : Editor
{
    private Vector2 scroll;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        StoreItem storeItem = (StoreItem)target;
        EditorGUILayout.LabelField("Description:");
        GUILayout.Space(-100);
        scroll = EditorGUILayout.BeginScrollView(scroll);
        storeItem.description = EditorGUILayout.TextField(storeItem.description,GUILayout.Width(200),GUILayout.Height(100));
        EditorGUILayout.EndScrollView();
        GUILayout.EndHorizontal();
        if(storeItem.sprite != null)
            EditorGUILayout.LabelField(new GUIContent(storeItem.sprite.texture),GUILayout.Width(50),GUILayout.Height(50));
    }   
}