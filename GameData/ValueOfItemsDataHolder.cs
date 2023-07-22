using System.Collections.Generic;
using UnityEngine;
public class ValueOfItemsDataHolder : ScriptableObject
{
        public List<SerializableElement<string>> valueOfItems;

        public void Initialize(Dictionary<string,int> valueOfItems)
        {
                this.valueOfItems = new List<SerializableElement<string>>();
                foreach (var element in valueOfItems)
                {
                        this.valueOfItems.Add(new SerializableElement<string>(element.Value,element.Key));
                }
        }
}
