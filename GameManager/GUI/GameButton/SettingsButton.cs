using UnityEngine.UI;

public sealed class SettingsButton : GameButton
{
    protected override void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetPopup);
    }

    protected override void SetPopup()
    {
        SoundManager.instance.PlaySound("Button");
        gameScene.OpenPopup<SettingsPopup>();
    }
}
