using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameScene : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private Canvas canvas;
    private Stack<GameObject> activePopups;
    public UnityEvent onPopupActivated;
    private Image fadePanel;
    protected virtual void Awake()
    {
        activePopups = new Stack<GameObject>();
        var panel = new GameObject("Panel");
        var panelImage = panel.AddComponent<Image>();
        var color = Color.black;
        color.a = 0;
        panelImage.color = color;
        var panelTransform = panel.GetComponent<RectTransform>();
        panelTransform.anchorMin = new Vector2(0, 0);
        panelTransform.anchorMax = new Vector2(1, 1);
        panelTransform.pivot = new Vector2(0.5f, 0.5f);
        panel.transform.SetParent(canvas.transform, false);
        fadePanel = panel.GetComponent<Image>();
        panel.SetActive(false);
    }

    public void OpenPopup<T>(Action<T> onOpened = null) where T : LvlWindowPopUp
    {
        EventBus.RaiseEvent<IGamePause>(input => input.Pause(this,true));
        EventBus.RaiseEvent<ITimerStop>(timer => timer.StopTimer());
        var popup = PopupHolderSingleton.Instance.SpawnPrefab(typeof(T),canvas);
        popup.gameScene = this;
        activePopups.Push(popup.gameObject);
        onOpened?.Invoke(popup.GetComponent<T>());
        onPopupActivated?.Invoke();
        Color32 color = fadePanel.color;
        color.a = 120;
        fadePanel.color = color;
        fadePanel.gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        var topmostPopup = activePopups.Pop();
        if (topmostPopup == null)
            return;
        Destroy(topmostPopup.gameObject);
        if (activePopups.Count > 0) return;
        EventBus.RaiseEvent<IGamePause>(input => input.Pause(this,false));
        fadePanel.gameObject.SetActive(false);
        EventBus.RaiseEvent<ITimerStop>(timer => timer.StartTimer());
    }
    public void LoadLevel(int lvlNumber)
    {
        sceneLoader.LoadLvl(lvlNumber,SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void RestartCurrentLevel()
    {
        GameSystemsManager.instance.advertisementSystem.ShowInterstitialAd(
            (sender, args) => sceneLoader.LoadLvl(SceneLoader.lvlNumber, SceneManager.GetActiveScene().buildIndex));

    }
    public void LoadNextLevel()
    {
        GameSystemsManager.instance.advertisementSystem.ShowInterstitialAd(
            (sender,eventArgs) => sceneLoader.LoadLvl(SceneLoader.lvlNumber + 1,SceneManager.GetActiveScene().buildIndex));
    }

    public void ToLevelMenu()
    {
        sceneLoader.LoadLvl(0,1);
    }
    
    public void ToStartScreen()
    {
        sceneLoader.LoadLvl(0,0);    
    }
}
