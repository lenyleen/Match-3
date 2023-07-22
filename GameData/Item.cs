using System;
using UnityEngine;


public class Item : ScriptableObject
{
    [field: SerializeField]public int ID { get; protected set; }
    
    [field: SerializeField]public string Name{ get; protected set; }
    [field: SerializeField]public int Count{ get; protected set; }
    private int count;
    public virtual void AddQuantity(int value)
    {
        Count += value;
    }

    public virtual void RemoveQuantity(int value)
    {
        
    }
}
