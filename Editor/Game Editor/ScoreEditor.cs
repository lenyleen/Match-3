using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ScoreEditor : IGameEditorWindow
{
    public string Name => "Score Editor";
    private List<Texture> textures;
    private Dictionary<string, int> valueOfItems;
    Vector2 scrollPosition = Vector2.zero;
    private const string assetPrefix = "ItemsValue";
    public ScoreEditor()
    {
        valueOfItems = new Dictionary<string, int>();
        var assetsPath = new DirectoryInfo("Assets/Resources/CandiesGridData");
        var existedData =
            (ValueOfItemsDataHolder) AssetDatabase.LoadAssetAtPath(
                assetsPath + $"/{assetPrefix}.asset", typeof(ValueOfItemsDataHolder));
        textures = new List<Texture>();
        var editorImagesPath = new DirectoryInfo(Application.dataPath + "/FruitCandiesMatch/Editor/Resources/GameItemsSprites");
        var fileInfo = editorImagesPath.GetFiles("*.png", SearchOption.TopDirectoryOnly);
        foreach (var file in fileInfo)
        {
            var filename = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            textures.Add(Resources.Load("GameItemsSprites/" + filename) as Texture);
        }
        if (existedData != null)
        {
            foreach (var element in existedData.valueOfItems)
            {
                valueOfItems.Add(element._obj,element.count);
            }
            return;
        }
        foreach (var texture in textures)
        {
            valueOfItems.Add(texture.name,0);
        }
    }

    public void DrawWindow()
    {
        EditorGUILayout.LabelField("Items value editor", EditorStyles.boldLabel);
        GUILayout.BeginVertical();
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < textures.Count; i++)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent(textures[i]), GUILayout.Width(20), GUILayout.Height(20));
            GUILayout.Space(15);
            EditorGUILayout.LabelField(textures[i].name);
            valueOfItems[textures[i].name] = EditorGUILayout.IntField(valueOfItems[textures[i].name]);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        if (GUILayout.Button("Save"))
        {
           SaveValueOfItems(); 
        }
        GUILayout.EndVertical();
    }

    private void SaveValueOfItems()
    {
        var dataHolder = ScriptableObject.CreateInstance<ValueOfItemsDataHolder>();
        dataHolder.Initialize(valueOfItems);
        AssetDatabase.CreateAsset(dataHolder, $"Assets/Resources/CandiesGridData/{assetPrefix}.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = dataHolder;
    }
}

