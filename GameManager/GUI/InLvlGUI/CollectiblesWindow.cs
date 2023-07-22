using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectiblesWindow : MonoBehaviour,IGoalCollectiblesObserver
{
    [SerializeField]private CollectibleImage collectiblesImagePrefab;
    private readonly Dictionary<string, CollectibleImage> collectibleImages = new Dictionary<string, CollectibleImage>();
    [SerializeField] private TextMeshProUGUI scoreGoal;
    private int scoreToReach;
    public event Action onScoreReached;
    public void Initialize(List<Element<Texture>> data, int scoreToReach, ScoreCounter scoreCounter)
    {
        if(data == null || data.Count <= 0)
        {
            scoreCounter.OnScoreChanged += UpdateScore;
            this.scoreToReach = scoreToReach;
            scoreGoal.gameObject.SetActive(true);
            scoreGoal.text = scoreToReach.ToString();
            return;
        }
        EventBus.Subscribe(this);
        for (int i = 0; i < data.Count; i++)
        {
            var newCollectibleImage = Instantiate(collectiblesImagePrefab, this.transform);
            newCollectibleImage.Initialize(data[i].Index0,data[i]._element);
            collectibleImages.Add(newCollectibleImage.NameOfCollectible,newCollectibleImage);
        }
    }

    private void UpdateScore(int score)
    {
        scoreToReach -= score;
        scoreGoal.text = scoreToReach.ToString();
        if (scoreToReach > 0) return;
        scoreGoal.text = 0.ToString();
        onScoreReached?.Invoke();
    }
    public void ItemCollected(string collectibleName,int remainingCount)
    {
        if(!collectibleImages.ContainsKey(collectibleName)) return;
        collectibleImages[collectibleName].UpdateState(remainingCount);
        if (remainingCount <= 0)
            collectibleImages.Remove(collectibleName);
        if (collectibleImages.Count > 0) return;
        onScoreReached?.Invoke();
    } 
    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }
}
