using System;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class GiftForPlayingTime : Gift
{
        private Button button => this.GetComponent<Button>();
        private event Action onButtonClick;
        [SerializeField] private Image collectedImage;
        public void Initialize(Sprite giftSprite, int ID, int count, bool collected)
        {
                base.Initialize(giftSprite, ID, count);
                button.enabled = false;
                if (collected)
                        collectedImage.enabled = true;
        }

        public void Activate(Action actionOnClick)
        { 
                button.enabled = true;
                onButtonClick += actionOnClick;
                button.onClick.AddListener(AddGiftToInventory);
        }
        public override void AddGiftToInventory()
        {
                base.AddGiftToInventory();
                collectedImage.enabled = true;
                onButtonClick?.Invoke();
                button.enabled = false;
                onButtonClick = null;
        }
}
