using System.Linq;
using UnityEngine;

public sealed class ItemsSystem
{
    private readonly string coinSystemGameId;
    private readonly string healthSystemGameId; 
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="coinSystemGameId">Id of coins system</param>
    /// <param name="healthSystemGameId">Id of health system</param>
    public ItemsSystem(string coinSystemGameId, string healthSystemGameId)
    {
        var items = ItemsDatabase.Instance.allItems.Where(item => !item.name.Contains("Shop"));
        foreach (var item in items)
        {
            if(!PlayerPrefs.HasKey($"{item.GameID}"))
            {
                item.SetAsPlayerPref();   
            }
        }
        this.coinSystemGameId = coinSystemGameId;
        this.healthSystemGameId = healthSystemGameId;
    }
    /// <summary>
    /// Adds item to player storage
    /// </summary>
    /// <param name="storeItemGameId">Item id</param>
    /// <param name="storeItemCount">Item amount</param>
    public void AddItem(string storeItemGameId, int storeItemCount)
    {
        if(!PlayerPrefs.HasKey($"{storeItemGameId}"))
            PlayerPrefs.SetInt($"{storeItemGameId}",0);
        if(storeItemGameId == coinSystemGameId)
        {
            GameSystemsManager.instance.coinsSystem.BuyCoins(storeItemCount);
            return;
        }
        if(storeItemGameId == healthSystemGameId)
        {
            GameSystemsManager.instance.healthSystem.AddHearth(storeItemCount);
            return;
        }
        var itemCount = PlayerPrefs.GetInt($"{storeItemGameId.ToString()}");
        itemCount += storeItemCount;
        PlayerPrefs.SetInt($"{storeItemGameId.ToString()}", itemCount);
    }
    /// <summary>
    /// Removes item from storage
    /// </summary>
    /// <param name="storeItemGameId"> Item id</param>
    /// <returns>Result of removing</returns>
    public bool RemoveItem(string storeItemGameId)
    {
        var countOfItemInStorage = PlayerPrefs.GetInt($"{storeItemGameId}");
        if (countOfItemInStorage <= 0)
            return false;
        countOfItemInStorage--;
        PlayerPrefs.SetInt($"{storeItemGameId}", countOfItemInStorage);
        return true;
    }
}
