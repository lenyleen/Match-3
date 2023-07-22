using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class LvlWindowPopUp : MonoBehaviour
{
    [HideInInspector] public GameScene gameScene;
    protected Animator animator;
    private const string closeTrigger = "Close";
    protected virtual void OnEnable()
    {
        SoundManager.instance.PlaySound("PopupOpen");
        animator = this.GetComponent<Animator>();
    }

    public virtual void OnClose()
    {
        SoundManager.instance.PlaySound("PopupClose");
        animator.SetTrigger(closeTrigger);
    }

    private void ClosePopUp()
    {
        if (gameScene == null) return;
        gameScene.ClosePopup();
    }
}
