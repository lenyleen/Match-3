using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class MainScreenSettingsButton : GameButton
{
    [SerializeField] private Toggle fxSoundEnable;
    [SerializeField] private Toggle bgSoundEnable;
    private bool buttonsEnabled = false;
    [SerializeField] private Sprite[] fxToggleSprites;
    [SerializeField] private Sprite[] bgToggleSprites;
    protected override void Start()
    {
#if !UNITY_EDITOR
        fxSoundEnable.isOn = SoundManager.instance.FXEnabled;
        bgSoundEnable.isOn = SoundManager.instance.BgEnabled;
#endif
        ChangeBgSound();
        ChangeFxSound();
        onClick.AddListener(EnableButtons);
    }
    private void EnableButtons()
    {
        buttonsEnabled = !buttonsEnabled;
        bgSoundEnable.gameObject.SetActive(buttonsEnabled);
        fxSoundEnable.gameObject.SetActive(buttonsEnabled);
    }

    public void ChangeFxSound()
    {
        SoundManager.instance.SetSoundFxEnabled(fxSoundEnable.isOn);
        fxSoundEnable.image.sprite = fxSoundEnable.isOn ? fxToggleSprites[1] : fxToggleSprites[0];
    }
    public void ChangeBgSound()
    {
        SoundManager.instance.SetMusicEnabled(bgSoundEnable.isOn);
        bgSoundEnable.image.sprite = bgSoundEnable.isOn ? bgToggleSprites[1] : bgToggleSprites[0];
    }
    protected override void SetPopup() {}
}
