using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Mathematics;

public class HearthOnLvlMenuCounter : MonoBehaviour
{
        [SerializeField] private Image hearthPrefab;
        [SerializeField] private Sprite activeHearth;
        [SerializeField] private Sprite inactiveHearth;
        private List<Image> availableHearthImage;
        [SerializeField] private TextMeshProUGUI timeToNextHearth;
        [SerializeField] private TextMeshProUGUI storedLivesCounter;
        [SerializeField] private ADButton adButton;
        private bool countingDown;
        private int maxHearthsCount;
        private void Start()
        {
                countingDown = false;
                maxHearthsCount = GameSystemsManager.instance.gameProperties.maxLives;
                availableHearthImage = new List<Image>();
                for (int i = 0; i < maxHearthsCount; i++)
                {
                        var newHearthImage = Instantiate(hearthPrefab, this.transform);
                        availableHearthImage.Add(newHearthImage);
                }
                adButton.OnUserEarnedReward += AddHearthAsReward;
                GameSystemsManager.instance.healthSystem.OnItemCountChanged += OnHearthCountChanged;
                CheckForNewHearth();
        }

        private void CheckForNewHearth()
        {
                var timeSpan = GameSystemsManager.instance.healthSystem.CheckForNewHearth();
                var activeHearthsCount = GameSystemsManager.instance.healthSystem.GetHealthInfo();
                if (activeHearthsCount > maxHearthsCount)
                {
                        var storedLives = activeHearthsCount - maxHearthsCount;
                        storedLivesCounter.enabled = true;
                        storedLivesCounter.text = $"+{storedLives.ToString()}";
                        activeHearthsCount = maxHearthsCount;
                }
                else
                        storedLivesCounter.enabled = false;
                for (int i = 0; i < activeHearthsCount; i++)
                {
                        availableHearthImage[i].sprite = activeHearth;
                }
                if (countingDown || activeHearthsCount >= maxHearthsCount) return;
                adButton.gameObject.SetActive(true);
                timeToNextHearth.text = timeSpan.ToString(format:@"m\:ss");
                StartCoroutine(SetTimer(timeSpan));
        }

        private void Update()
        {
                if(Input.GetKeyDown(KeyCode.N))
                        GameSystemsManager.instance.healthSystem.RemoveHearth();
                if(Input.GetKeyDown(KeyCode.V))
                        GameSystemsManager.instance.healthSystem.AddHearth(1);
        }

        private void OnHearthCountChanged(int count)
        {
                if (count >= maxHearthsCount)
                {
                        adButton.gameObject.SetActive(false);
                        StopAllCoroutines();
                        countingDown = false;
                        timeToNextHearth.enabled = false;
                }
                if(count <= 0)
                {
                        var countOfHearths = availableHearthImage.Count - 1;
                        count = Mathf.Abs(count) < countOfHearths ? count : countOfHearths;
                        for (int i = countOfHearths; i >= Mathf.Abs(count); i--)
                        {
                                availableHearthImage[i].sprite = inactiveHearth;
                        }
                }
                CheckForNewHearth();
        }

        private IEnumerator SetTimer(TimeSpan currentRemainingTime)
        {
                countingDown = true;
                timeToNextHearth.enabled = true;
                while (currentRemainingTime > TimeSpan.Zero)
                {
                        yield return new WaitForSeconds(1f);
                        currentRemainingTime = currentRemainingTime.Subtract(TimeSpan.FromSeconds(1));
                        timeToNextHearth.text = currentRemainingTime.ToString(@"m\:ss");
                }
                CheckForNewHearth();
                countingDown = false;
        }

        private void AddHearthAsReward()
        {
            GameSystemsManager.instance.healthSystem.AddHearth();    
        }

        private void OnDisable()
        {
                GameSystemsManager.instance.healthSystem.Unsubscribe(OnHearthCountChanged);
        }
}
