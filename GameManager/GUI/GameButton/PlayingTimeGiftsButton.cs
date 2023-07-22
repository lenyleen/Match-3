using UnityEngine.UI;

public sealed class PlayingTimeGiftsButton : GameButton
{
    protected override void Awake()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(SetPopup);
    }

    protected override void SetPopup()
    {
        SoundManager.instance.PlaySound("Button");
        gameScene.OpenPopup<PlayingTimeGiftsPopup>();
    }
}
