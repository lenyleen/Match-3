using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;


public sealed class LvlCompleteWindowPopUp : LvlWindowPopUp
{
        [SerializeField] private Image[] emptyStars;
        [SerializeField] private Sprite earnedStar;
        private List<StoreItem> dataOfReward;

        [SerializeField] private Text scoreText;
        [SerializeField] private Text lvlInfo;
        
        [SerializeField] private Gift rewardPrefab;
        [SerializeField] private Transform rewardGridTransform;
        
        
        [SerializeField] private Button nextLvlButton;
        [SerializeField] private Button restartLvlButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private ADButton watchADButton;
        public void Initialize(int score, int earnedStarsCount, int lvlNumber, List<StoreItem> dataOfReward)
        {
            SoundManager.instance.PlaySound("AwardPopup");
            scoreText.text = $"SCORE: {score}";
            for (int i = 0; i < earnedStarsCount; i++)
            {
                emptyStars[i].sprite = earnedStar;
            }
            lvlInfo.text = $"LVL {lvlNumber} COMPLETED!";
            this.dataOfReward = dataOfReward;
            for (int i = 0; i < dataOfReward.Count; i++)
            {
                var newReward = Instantiate(rewardPrefab, rewardGridTransform);
                newReward.Initialize(dataOfReward[i].sprite,dataOfReward[i].GameID,1);
            }
            GameSystemsManager.instance.lvlSystem.SetLvlAsPassed(lvlNumber,earnedStarsCount,score);
            watchADButton.OnUserEarnedReward += GiveReward;
            nextLvlButton.onClick.AddListener(gameScene.LoadNextLevel);
            restartLvlButton.onClick.AddListener(gameScene.RestartCurrentLevel);
            mainMenuButton.onClick.AddListener(OnClose);
        }

        private void GiveReward()
        {
            foreach (var reward in dataOfReward)
            {
                GameSystemsManager.instance.itemsSystem.AddItem(reward.GameID.ToString(),1);
            }
            watchADButton.gameObject.SetActive(false);
        }
        public override void OnClose()
        {
            gameScene.ToLevelMenu();
        }
}
