using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LvlStartWindowPopUpPopUp : LvlWindowPopUp
{
        private int lvlNumber;
        [SerializeField] private CollectibleImage prefabOfGoalItem;
        [SerializeField] private Transform goalGridTransform;
        [SerializeField] private List<StoreItem> shopItems;
        [SerializeField] private PurchasedItem purchasedItemPrefab;
        [SerializeField] private Transform purchasedItemsGrid;
        [SerializeField] private Text lvlNumberText;
        [SerializeField] private TextMeshProUGUI scoreGoal;
        public void Initialize(int lvlNumber)
        {
                this.lvlNumber = lvlNumber;
                lvlNumberText.text = $"LEVEL {lvlNumber}";
                var data = CandiesGridDataSaver.LoadData(lvlNumber);
                foreach (var item in shopItems)
                {
                        var newPurchasedItem = Instantiate(purchasedItemPrefab, purchasedItemsGrid);
                        newPurchasedItem.Initialize(item);
                }
                if (data.dataOfCollectiblesAsGoal == null || data.dataOfCollectiblesAsGoal.Count <= 0)
                {
                        scoreGoal.text = $"SCORE: {data.scoreGoal}";
                        scoreGoal.gameObject.SetActive(true);
                        return;
                }
                foreach (var element in data.dataOfCollectiblesAsGoal)
                {
                        var newGoalItem = Instantiate(prefabOfGoalItem, goalGridTransform);
                        newGoalItem.Initialize(element.Index0,element._element);
                }
        }

        public void LoadLvl()
        {
                var currentCountOfHearths = GameSystemsManager.instance.healthSystem.GetHealthInfo();
                if(currentCountOfHearths <= 0)
                {
                        gameScene.OpenPopup<NotEnoughHearthsPopUp>();
                        return;
                }
                gameScene.LoadLevel(lvlNumber);
        }
}
