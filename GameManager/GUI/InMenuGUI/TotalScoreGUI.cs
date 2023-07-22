using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TotalScoreGUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerLvl;
    [SerializeField] private TextMeshProUGUI playerTotalScore;
    [SerializeField] private Image scoreFillingBar;
    private ScoreSystem scoreSystem;

    private void Start()
    {
        scoreSystem = GameSystemsManager.instance.scoreSystem;
        CalculateFillingBarAmount();
    }

    private void CalculateFillingBarAmount()
    {
        var totalScore = scoreSystem.totalScore;
        var totalScoreToNextLvl = scoreSystem.toNextLvlTotalScore;
        var delta = totalScore - totalScoreToNextLvl;
        float percentOfCurrentLvl = delta != 0 ? (totalScore * 100) / totalScoreToNextLvl : 0;
        float percent = percentOfCurrentLvl / 100;
        scoreFillingBar.fillAmount = percent;
        playerLvl.text = scoreSystem.playerLvl.ToString();
        playerTotalScore.text = totalScore.ToString();
    }
}
