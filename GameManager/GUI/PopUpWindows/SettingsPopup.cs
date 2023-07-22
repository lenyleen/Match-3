using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class SettingsPopup : LvlWindowPopUp
{
    [SerializeField] private Button nextLvlButton;
    [SerializeField] private Button restartLvlButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Toggle fxEnable;
    [SerializeField] private Toggle bgEnable;
    [SerializeField] private Slider fxSoundSlider;
    [SerializeField] private Slider bgSoundSlider;

    private void Start()
    {
        if (gameScene == null) return;
        var soundManager = SoundManager.instance;
        fxSoundSlider.value = soundManager.FXVolume;
        bgSoundSlider.value = soundManager.BgVolume;
        fxEnable.isOn = soundManager.FXEnabled;
        bgEnable.isOn = soundManager.BgEnabled;
        fxSoundSlider.value = soundManager.FXVolume;
        bgSoundSlider.value = soundManager.BgVolume;
        fxEnable.onValueChanged.AddListener(soundManager.SetSoundFxEnabled);
        bgEnable.onValueChanged.AddListener(soundManager.SetMusicEnabled); 
        fxSoundSlider.onValueChanged.AddListener(soundManager.ToggleSound);
        bgSoundSlider.onValueChanged.AddListener(soundManager.ToggleMusic);
        nextLvlButton.onClick.AddListener(gameScene.LoadNextLevel);
        restartLvlButton.onClick.AddListener(gameScene.RestartCurrentLevel);
        mainMenuButton.onClick.AddListener(OpenWarningPopUp);
    }

    private void OpenWarningPopUp()
    {
        gameScene.OpenPopup<LvlLeaveWarningPopUp>();   
    }
}
