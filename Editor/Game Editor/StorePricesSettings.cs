using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class StorePricesSettings : IGameEditorWindow
{
    public string Name => "Store Prices Editor";
    private List<StoreItem> storeItems;
    private const string assetPrefix = "StorePrices";
    public StorePricesSettings()
    {
        var items = Resources.LoadAll<StoreItem>("BuyItems/");
        storeItems = new List<StoreItem>();
        for (int i = 0; i < items.Length; i++)
        {
            storeItems.Add(items[i]);
            storeItems[i].ShopID = i + 1;
        }
    }

    public void DrawWindow()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(100);
        EditorGUILayout.LabelField("Name:",GUILayout.Width(100),GUILayout.Height(50));
        EditorGUILayout.LabelField("Texture:",GUILayout.Width(100),GUILayout.Height(50));
        EditorGUILayout.LabelField("ID:",GUILayout.Width(100),GUILayout.Height(50));
        EditorGUILayout.LabelField("Count:",GUILayout.Width(100),GUILayout.Height(50));
        EditorGUILayout.LabelField("Price:",GUILayout.Width(100),GUILayout.Height(50));
        EditorGUILayout.LabelField("Currency:",GUILayout.Width(100),GUILayout.Height(50));
        GUILayout.EndHorizontal();
        for (int i = 0; i < storeItems.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            EditorGUILayout.LabelField(storeItems[i].Name,GUILayout.Width(100),GUILayout.Height(50));
            EditorGUILayout.LabelField(new GUIContent(storeItems[i].sprite.texture),GUILayout.Width(50), GUILayout.Height(50));
            GUILayout.Space(10);
            EditorGUILayout.IntField(storeItems[i].ShopID, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.Space(10);
            storeItems[i].Count = EditorGUILayout.IntField(storeItems[i].Count, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.Space(10);
            storeItems[i].price =
                EditorGUILayout.FloatField(storeItems[i].price, GUILayout.Width(100), GUILayout.Height(20));
            storeItems[i].currency =
                (Currency)EditorGUILayout.EnumPopup(storeItems[i].currency, GUILayout.Width(100), GUILayout.Height(20));
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Save"))
        {
            SaveValueOfItems(); 
        }
    }
    private void SaveValueOfItems()
    {
        var dataHolder = ScriptableObject.CreateInstance<StorePricesHolder>();
        dataHolder.Save(storeItems);
        AssetDatabase.CreateAsset(dataHolder, $"Assets/CandiesGridData/{assetPrefix}.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = dataHolder;
    }
}
