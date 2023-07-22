using System;
using System.Linq;
using UnityEngine;

public class GameSystemsManager : MonoBehaviour
{
    public static GameSystemsManager instance;
    public CoinsSystem coinsSystem;
    public HealthSystem healthSystem;
    [HideInInspector]public GameProperties gameProperties;
    public LvlSystem lvlSystem;
    public ScoreSystem scoreSystem;
    public ItemsSystem itemsSystem;
    public AdvertisementSystem advertisementSystem;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)) 
            coinsSystem.BuyCoins(100);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        gameProperties = Resources.Load<GameProperties>("CandiesGridData/GameConfiguration");
        var items = ItemsDatabase.Instance.allItems.Where(item => !item.name.Contains("Shop"));
        var coinSystemGameId = items.FirstOrDefault(item => item.Name.Contains("Gold")).GameID;
        var healthSystemGameId = items.FirstOrDefault(item => item.Name.Contains("Hearth")).GameID;
        coinsSystem = new CoinsSystem(coinSystemGameId);
        healthSystem = new HealthSystem(healthSystemGameId);
        lvlSystem = new LvlSystem();
        scoreSystem = new ScoreSystem();
        itemsSystem = new ItemsSystem(coinSystemGameId.ToString(), healthSystemGameId.ToString());
        advertisementSystem = new AdvertisementSystem();
    }
}
