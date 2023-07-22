using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class DailyGiftsPopUp : LvlWindowPopUp
{
    [SerializeField]private DailyGift giftPrefab;
    [SerializeField] private Transform giftsGridTransform;

    public void Initialize(Action action, bool giftAvailable)
    {
        var giftsData = ItemsDatabase.Instance.dailyGiftsData.FirstOrDefault(data => (int)data.month == DateTime.Now.Month);
        if(giftsData == null)
        {
            OnClose();
            return;
        }
        for (int i = 0; i < giftsData.giftItemsID.Count; i++)
        {
            var itemOfData = giftsData.giftItemsID[i];
            var newGift = Instantiate(giftPrefab, giftsGridTransform);
            if(i == DateTime.Now.Day && giftAvailable)
                newGift.Initialize(itemOfData.sprite,itemOfData.GameID,itemOfData.Count,i + 1,true,action);
            newGift.Initialize(itemOfData.sprite,itemOfData.GameID,itemOfData.Count,i + 1,false,null);
        }
        
    }
}
