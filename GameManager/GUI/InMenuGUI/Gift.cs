using UnityEngine;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{
        [field:SerializeField] public Image imageOfGift { get; private set;}
        public int count { get; private set; }
        [SerializeField] protected Text countText;

        public int ID { get; private set; }

        public void Initialize(Sprite giftSprite, int ID, int count)
        {
                imageOfGift.sprite = giftSprite;
                float spritesDelta = (float)giftSprite.rect.width / imageOfGift.rectTransform.sizeDelta.y;
                imageOfGift.SetNativeSize();
                var imageSize = imageOfGift.rectTransform.sizeDelta;
                var spriteWidth = imageSize.x / spritesDelta;
                var spriteHeight = imageSize.y / spritesDelta;
                imageOfGift.rectTransform.sizeDelta = new Vector2(spriteWidth - 10, spriteHeight - 10);
                this.ID = ID;
                this.count = count;
                countText.text = count.ToString();
        }

        public virtual void AddGiftToInventory()
        { 
                GameSystemsManager.instance.itemsSystem.AddItem(ID.ToString(),count);
        }
}
