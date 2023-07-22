using System;
using UnityEngine;

public class CoinsSystem : IPublisher
{
    public event Action<int> OnItemCountChanged;
    private int coinsGameId;
    public int totalCoins => PlayerPrefs.GetInt(coinsGameId.ToString()); 

    public CoinsSystem(int coinsGameId)
    {
        if(!PlayerPrefs.HasKey(coinsGameId.ToString()))
            PlayerPrefs.SetInt(coinsGameId.ToString(),GameSystemsManager.instance.gameProperties.initialCoins);
        this.coinsGameId = coinsGameId;
    }
    
    public void BuyCoins(int amount)
    {
        var numCoins = PlayerPrefs.GetInt(coinsGameId.ToString());
        numCoins += amount;
        PlayerPrefs.SetInt(coinsGameId.ToString(), numCoins);
        Debug.Log("added");
        OnItemCountChanged?.Invoke(numCoins);
    }
    public bool SpendCoins(int amount)
    {
        var numCoins = PlayerPrefs.GetInt(coinsGameId.ToString());
        numCoins -= amount;
        if (numCoins < 0)
            return false;
        PlayerPrefs.SetInt(coinsGameId.ToString(), numCoins);
        OnItemCountChanged?.Invoke(numCoins);
        return true;
    }

    public void Subscribe(Action<int> callback)
    {
        OnItemCountChanged += callback;
    }

    public void Unsubscribe(Action<int> callback)
    {
        if (OnItemCountChanged != null)
        {
            OnItemCountChanged -= callback;
        }
    }
}
