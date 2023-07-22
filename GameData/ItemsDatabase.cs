
using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemsDatabase", menuName = "ItemsDatabase/Database")]
public class ItemsDatabase : SingletonScriptableObject<ItemsDatabase>
{
        public List<StoreItem> allItems;
        public List<DailyGiftsData> dailyGiftsData;
        public List<StoreItem> giftForPlayingTimeToday;
        private void OnEnable()
        {
                dailyGiftsData = new List<DailyGiftsData>();
                var items = Resources.LoadAll<StoreItem>("CandiesGridData/BuyItems/"); 
                Debug.Log($"{items.Length}");
                foreach (var item in items)
                {
                        if(!allItems.Contains(item))
                                allItems.Add(item);
                }

                var dailyGifts = Resources.LoadAll<DailyGiftsData>("CandiesGridData/DailyGiftData/");
                foreach (var item in dailyGifts)
                {
                        dailyGiftsData.Add(item);
                }
                CleanDeletedData();
        }

        private void CleanDeletedData()
        {
                foreach (var item in allItems)
                {
                        if (item == null)
                                allItems.Remove(item);
                }

                foreach (var item in dailyGiftsData)
                {
                        if (item == null)
                                dailyGiftsData.Remove(item);
                }
        }
}
