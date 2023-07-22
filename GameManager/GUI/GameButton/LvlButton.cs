using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LvlButton : GameButton
{
    private bool lvlAvailable;
    private int lvlNumber;
    private int LvlNumber
    {
        get => lvlNumber;
        set
        {
            lvlNumber = value;
            lvlNumberText.text = lvlNumber.ToString();
        }
    }
    [SerializeField] private TextMeshProUGUI lvlNumberText;
    [SerializeField] private List<Image> stars;
    [SerializeField] private Sprite goldenStar;
    public void Initialize(bool lvlAvailable, int lvlNumber, int countOfStars, GameScene gameScene)
    {
        button.onClick.AddListener(SetPopup);
        this.lvlAvailable = lvlAvailable;
        this.LvlNumber = lvlNumber;
        this.gameScene = gameScene;
        for (int i = 0; i < countOfStars; i++)
        {
            stars[i].sprite = goldenStar;
        }
    }
    protected override void SetPopup()
    {
        SoundManager.instance.PlaySound("PopupOpenButton");
        if(lvlAvailable)
        {
            gameScene.OpenPopup<LvlStartWindowPopUpPopUp>(popup => popup.Initialize(lvlNumber));
            return;
        }
        gameScene.OpenPopup<LockedLvlWindowPopUpPopUp>();
    }
}
