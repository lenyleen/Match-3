using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameItem", menuName = "GameItem")]
public class StoreItem : ScriptableObject
{
    [field: SerializeField]public int GameID { get; private set; }
    [field: SerializeField] public int ShopID { get; set; }
    [field: SerializeField]public string Name{ get; private set; }
    [field: SerializeField]public int Count{ get; set; }
    [field: SerializeField] public Sprite sprite { get; private set; }
    [HideInInspector]public string description;
    [field:SerializeField] public BoosterType boosterType { get; private set;} 
    [field: SerializeField]public float price{ get;  set; }
    [field: SerializeField]public Currency currency{ get; set; }
    [field: SerializeField]public int dropChance{ get; private set; }

    public void SetAsPlayerPref()
    {
        PlayerPrefs.SetInt(GameID.ToString(),Count);
    }
    
}

public enum Currency
{
     Gold,
     Donate
}
