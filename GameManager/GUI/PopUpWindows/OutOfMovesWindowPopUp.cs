using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class OutOfMovesWindowPopUp : LvlWindowPopUp
{
    public Action<bool> onPaymentComplete;
    [SerializeField] private Text description;
    protected override void OnEnable()
    {
        SoundManager.instance.PlaySound("NoMovesOrTimeAlert");
    }

    public void Initialize(bool movesAsLimiter, Action<bool> onPaymentComplete)
    {
        var insert = movesAsLimiter ? "MOVES" : "TIME";
        description.text = $"IT SEEMS YOUR {insert} IS OVER" + $"\r\n\nADD {insert}";
        this.onPaymentComplete = onPaymentComplete;
    }

    public void WatchAd()
    {
        Debug.Log("ad watched");
        onPaymentComplete.Invoke(true);
    }

    public void PayGold()
    {
        Debug.Log("gold payed");
        onPaymentComplete.Invoke(true);
    }

    public override void OnClose()
    {
        base.OnClose();
        onPaymentComplete.Invoke(false);
    }
}
