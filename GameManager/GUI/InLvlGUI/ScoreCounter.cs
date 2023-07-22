using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour,IScoreItemObserver
{
    [SerializeField] private GameObject starPrefab;
    [SerializeField]private Image scoreBarFiller;
    private Dictionary<string, int> valueOfItems;
    private List<float> starsPercents;
    private GUIStar[] stars;
    private float maxScore;
    private bool coroutineIsRunning;
    [SerializeField] private float addedScore = 50;
    private float addedPercent;
    private int starsCounter;
    public float CompleteScore { get; private set; }

    public int CompleteCountOfStars
    {
        get
        {
            int countOfStars = 0;
            var reachedPercent = CompleteScore / maxScore;
            for (var i = 0; i < starsPercents.Count; i++)
            {
                if(!(reachedPercent >= starsPercents[i])) continue;
                countOfStars++;
            }
            return countOfStars;
        }
    }
    public event Action<int> OnScoreChanged;
    private const float ToPercentConst = 0.01f;
    public void Initialize(float maxScore, List<int> scoreStarsPercents, Dictionary<string, int> valueOfItems)
    {
        starsPercents = scoreStarsPercents.Select(starPercent => starPercent * ToPercentConst).ToList();
        EventBus.Subscribe(this);
        coroutineIsRunning = false;
        this.maxScore = maxScore;
        stars = new GUIStar[3];
        this.valueOfItems = valueOfItems;
        addedPercent = addedScore / maxScore;
        for (int i = 0; i < 3; i++)
        {
            stars[i] = SpawnStar(starsPercents[i]);
        }
    }
    private GUIStar SpawnStar(float percent)
    {
        var newStar = Instantiate(starPrefab, this.transform).GetComponent<GUIStar>();
        var thisRect = this.GetComponent<RectTransform>();
        newStar.GetComponent<RectTransform>().anchoredPosition = new Vector2(thisRect.rect.width * percent, 0);
        return newStar;
    }
    private IEnumerator ScoreBarAnimation()
    {
        coroutineIsRunning = true;
        var percentToReach = CompleteScore / maxScore;
        
        while (scoreBarFiller.fillAmount < percentToReach)
        {
            scoreBarFiller.fillAmount += addedPercent; 
            for (int i = starsCounter; i < starsPercents.Count; i++)
            {
                if (!(scoreBarFiller.fillAmount >= starsPercents[i])) continue;
                stars[i].Activate();
                starsCounter++;
            }
            yield return new WaitForSeconds(0.02f); 
        }
        percentToReach = CompleteScore / maxScore;
        coroutineIsRunning = false;
        if (scoreBarFiller.fillAmount < percentToReach)
            StartCoroutine(ScoreBarAnimation());
        
    }
    public void ItemCollected(string nameOfItem)
    {
        var scoreToAdd = valueOfItems[nameOfItem];
        CompleteScore += scoreToAdd;
        if (!coroutineIsRunning || scoreBarFiller.fillAmount < 1f)
        {
            StartCoroutine(ScoreBarAnimation());
        }
        OnScoreChanged?.Invoke(scoreToAdd);
    } 
    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

   
}
