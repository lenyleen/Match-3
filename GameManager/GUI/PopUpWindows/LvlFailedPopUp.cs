using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class LvlFailedPopUp : LvlWindowPopUp
{
    [SerializeField] private Image[] emptyStars;
    [SerializeField] private Sprite earnedStar;
    [SerializeField] private Text scoreText;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Text lvlInfo;

    public void Initialize(int score, int earnedStarsCount, int lvlNumber)
    {
        SoundManager.instance.PlaySound("Lose");
        lvlInfo.text = $"OOPS..LEVEL {lvlNumber} FAILED!";
        menuButton.onClick.AddListener(gameScene.ToLevelMenu);
        restartButton.onClick.AddListener(gameScene.RestartCurrentLevel);
        scoreText.text = $"SCORE: {score}";
        GameSystemsManager.instance.healthSystem.RemoveHearth();
        for (int i = 0; i < earnedStarsCount; i++)
        {
            emptyStars[i].sprite = earnedStar;
        }
    }
    public override void OnClose()
    {
        gameScene.ToLevelMenu();
    }
}
