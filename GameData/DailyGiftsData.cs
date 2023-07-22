using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class DailyGiftsData : ScriptableObject
{
    public MonthOfYear month;
    public List<StoreItem> giftItemsID;
}

public enum MonthOfYear
{
    January = 1,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December
}
