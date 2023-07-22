using System.Collections.Generic;
using UnityEngine;

public class StorePricesHolder : ScriptableObject
{
      public List<StoreItem> storeItems;

      public void Save(List<StoreItem> storeItems)
      {
            this.storeItems = storeItems;
      }
}
