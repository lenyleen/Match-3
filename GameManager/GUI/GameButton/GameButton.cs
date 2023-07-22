using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameButton : Button
{
        [SerializeField]protected Button button;
        [SerializeField]protected GameScene gameScene;
        protected abstract void SetPopup();
}

