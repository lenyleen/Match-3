using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class BuyBoosterPopup : LvlWindowPopUp
{
    [SerializeField] private Image boosterImage;
    private int price;
    private int count;
    [SerializeField] private Text description;
    [SerializeField] private Text nameOfBooster;
    [SerializeField] private Text countOfBooster;
    [SerializeField] private Text priceText;
    [SerializeField] private Text currentPlayersGold;
    private event Action onBoughtAction;

    public void SetBooster(StoreItem booster, Action action)
    {
        boosterImage.sprite = booster.sprite;
        price = (int)booster.price;
        priceText.text = price.ToString();
        count = booster.Count;
        onBoughtAction += action;
        countOfBooster.text = $"+{count}";
        currentPlayersGold.text = GameSystemsManager.instance.coinsSystem.totalCoins.ToString();
        description.text = booster.description;
        nameOfBooster.text = booster.Name;
        boosterImage.SetNativeSize();
    }

    public void BuyItem()
    {
        var purchaseResult = GameSystemsManager.instance.coinsSystem.SpendCoins(price);
        if (!purchaseResult) return;
        onBoughtAction?.Invoke();  
        onBoughtAction = null;
        OnClose();
    }
}
