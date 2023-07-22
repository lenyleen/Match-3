using UnityEngine;

public class LvlScene : GameScene
{
    [SerializeField] private GameManager gameManager;

    protected void Start()
    {
        base.Awake();
        gameManager.Initialize(SceneLoader.lvlNumber);
    }
    public void LoadNextLvl()
    {
        Debug.Log("to next lvl");
    }

    public void RestartCurrentLvl()
    {
        Debug.Log("restarted");
    }

    public void GoToMainMenu()
    {
        Debug.Log("to main menu");
    }
}
