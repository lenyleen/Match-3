using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class DailyGiftsEditor : IGameEditorWindow
{
    public string Name => "Daily Gift Editor";
    private float daysInCurrentMonth;
    private int redundantDays;
    private int countOfRows;
    private StoreItem storeItem;
    private List<GUIContent> labels;
    private DailyGiftsData currentDataOfMonth;
    private List<DailyGiftsData> giftsData;
    private MonthOfYear currentMonth;
    private MonthOfYear monthToChange;
    public DailyGiftsEditor()
    {
        var existedDailyGiftsData = ItemsDatabase.Instance.dailyGiftsData;
        if (existedDailyGiftsData != null && existedDailyGiftsData.Count > 0)
        {
            giftsData = existedDailyGiftsData;
            if (giftsData[0].giftItemsID.Count <= 0)
                giftsData[0].giftItemsID =
                    new List<StoreItem>(DateTime.DaysInMonth(DateTime.Now.Year, (int) giftsData[0].month));
            currentDataOfMonth = giftsData[0];
        }
        else
        {
            currentDataOfMonth = ScriptableObject.CreateInstance<DailyGiftsData>();
            currentDataOfMonth.month = (MonthOfYear) DateTime.Now.Month;
            currentDataOfMonth.giftItemsID =
                new List<StoreItem>(DateTime.DaysInMonth(DateTime.Now.Year, (int) currentDataOfMonth.month));
            AssetDatabase.CreateAsset(currentDataOfMonth,
                $"Assets/Resources/BuyItems/{currentDataOfMonth.month}.asset");
            AssetDatabase.SaveAssets();
            giftsData = new List<DailyGiftsData>() {currentDataOfMonth};
        }
        currentMonth = currentDataOfMonth.month;
        monthToChange = currentMonth;
        daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
        countOfRows = Mathf.CeilToInt(daysInCurrentMonth / 7);
        redundantDays = (int) daysInCurrentMonth - 28;
        labels = new List<GUIContent>();
    }
    public void DrawWindow()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(300);
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("DAILY GIFT EDITOR",EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Month:");
        monthToChange = (MonthOfYear)EditorGUILayout.EnumPopup(monthToChange,GUILayout.Width(100));
        if (GUILayout.Button("Change month",GUILayout.Width(150)))
            ChangeMonth(monthToChange);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        var k = 0;
        for (int i = 0; i < countOfRows; i++)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < 7; j++)
            { 
                if(labels.Count <= k)
                    labels.Add(new GUIContent());
                if(currentDataOfMonth.giftItemsID.Count <= k)
                    currentDataOfMonth.giftItemsID.Add(null);
                if (currentDataOfMonth.giftItemsID[k] != null)
                    labels[k] = new GUIContent(currentDataOfMonth.giftItemsID[k].sprite.texture);
                GUILayout.BeginVertical();
                GUILayout.Box(labels[k], GUILayout.Width(50), GUILayout.Height(50));
                currentDataOfMonth.giftItemsID[k] = (StoreItem) EditorGUILayout.ObjectField(
                    currentDataOfMonth.giftItemsID[k], typeof(StoreItem), false, GUILayout.Width(50), GUILayout.Height(20));
                GUILayout.EndVertical();
                if (i == countOfRows - 1 && j == redundantDays - 1 && redundantDays != 0)
                    break;
                k++;
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save data"))
        {
            SaveData();
        }
    }

    /*private List<StoreItem> SetGiftItemsOfMonth(List<int> itemsID)
    {
        var convertedItems = new List<StoreItem>();
        for (int i = 0; i < itemsID.Count; i++)
        {
            convertedItems.Add(dataOfStoreItems.FirstOrDefault(item => item.ShopID == itemsID[i]));
        }
        return convertedItems;
    }

    private List<int> SetItemsIdOfMonth(List<StoreItem> items)
    {
        var convertedItems = new List<int>();
        for (int i = 0; i < items.Count; i++)
        {
            convertedItems.Add(items[i].ShopID);
        }
        return convertedItems;
    }*/

    private void SaveData()
    {
        EditorUtility.SetDirty(currentDataOfMonth);
        AssetDatabase.Refresh();
    }
    private void ChangeMonth(MonthOfYear monthOfYear)
    {
        SaveData();
        currentMonth = monthOfYear;
        var dataOfMonth = giftsData.FirstOrDefault(data => data.month == currentMonth);
        if (dataOfMonth != null)
        {
            if(dataOfMonth.giftItemsID.Count <= 0)
                dataOfMonth.giftItemsID = new List<StoreItem>(DateTime.DaysInMonth(DateTime.Now.Year,(int)dataOfMonth.month));
            currentDataOfMonth = dataOfMonth;
            return;
        }
        currentDataOfMonth = ScriptableObject.CreateInstance<DailyGiftsData>();
        currentDataOfMonth.month = currentMonth;
        currentDataOfMonth.giftItemsID = new List<StoreItem>(DateTime.DaysInMonth(DateTime.Now.Year,(int)currentDataOfMonth.month));
        giftsData.Add(currentDataOfMonth);
        AssetDatabase.CreateAsset(currentDataOfMonth, $"Assets/Resources/BuyItems/{currentDataOfMonth.month}.asset");
        daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Today.Year,(int)currentMonth);
        countOfRows = Mathf.CeilToInt(daysInCurrentMonth / 7);
        redundantDays = (int) daysInCurrentMonth - 28;
        AssetDatabase.SaveAssets();
    }
}
