using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PurchasedItem : MonoBehaviour
{
        [SerializeField] private Text priceText;
        [SerializeField] private Image imageOfItem;
        [SerializeField] private Text count;
        private int price;
        private int itemGameId;
        private const int ItemCount = 1;
        public void Initialize(StoreItem item)
        {
                price = (int)item.price / 2;
                priceText.text = price.ToString();
                itemGameId = item.GameID;
                imageOfItem.sprite = item.sprite;
                imageOfItem.SetNativeSize();
                count.text = ItemCount.ToString();
        }

        public void BuyItem()
        {
                var paymentResult = GameSystemsManager.instance.coinsSystem.SpendCoins(price);
                if(paymentResult)
                        GameSystemsManager.instance.itemsSystem.AddItem(itemGameId.ToString(),ItemCount);
        }
}
