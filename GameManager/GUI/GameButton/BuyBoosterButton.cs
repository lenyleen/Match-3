using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class BuyBoosterButton : GameButton
{
        [field:SerializeField] public StoreItem booster { get; private set;}
        [SerializeField] private Image boosterImage;
        private int Count
        {
                get => count;
                set
                {
                        count = value;
                        countText.text = count.ToString();
                }
        }

        private int count;
        [SerializeField] private Text countText;
        protected override void Awake()
        {
                Count = PlayerPrefs.GetInt($"{booster.GameID}");
                boosterImage.sprite = booster.sprite;
                button.onClick.AddListener(SetPopup);
        }

        protected override void SetPopup()
        {
                SoundManager.instance.PlaySound("BuyPopButton");
                if (count > 0)
                {
                        EventBus.RaiseEvent<IBoosterActivated>(listener => listener.BoosterActivated(booster.boosterType,BoosterWasActivated));
                        return;
                }
                gameScene.OpenPopup<BuyBoosterPopup>(popup => { popup.SetBooster(booster, ItemWasBought);});
        }

        private void BoosterWasActivated()
        {
                GameSystemsManager.instance.itemsSystem.RemoveItem(booster.GameID.ToString());
                Count--;    
        }

        private void ItemWasBought()
        {
                Count += booster.Count;
                GameSystemsManager.instance.itemsSystem.AddItem(booster.GameID.ToString(), booster.Count);
        }
}
