using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class WheelOfFortuneButton : GameButton
{
    private bool active;
    private int afterSpinWheelActiveNextTime = 24;
    [SerializeField] private Text timer;
    private new Animator animator => this.GetComponent<Animator>();
    protected override void Start()
    {
        if (!PlayerPrefs.HasKey("wheel_of_fortune_timer"))
        {
            active = true;
            PlayerPrefs.SetString("wheel_of_fortune_timer",DateTime.Now.ToBinary().ToString());
        }
        var nextTimeActiveWheel = CheckWheelToActive();
        StartCoroutine(StartTimer(nextTimeActiveWheel));
        button.onClick.AddListener(SetPopup);
    }

    private TimeSpan CheckWheelToActive()
    {
        var wheelNextTimeActiveBinary = PlayerPrefs.GetString("wheel_of_fortune_timer");
        var binaryDate = Convert.ToInt64(wheelNextTimeActiveBinary);
        var prevNextWheelTime = DateTime.FromBinary(binaryDate);
        TimeSpan remainingTime = TimeSpan.Zero;
        var now = DateTime.Now;
        if (prevNextWheelTime > now)
        {
            active = false;
            remainingTime = prevNextWheelTime - now;
        }
        else
        {
            active = true;
        }
        return remainingTime;
    }
    private IEnumerator StartTimer(TimeSpan currentRemainingTime)
    {
        while (currentRemainingTime > TimeSpan.Zero)
        {
            yield return new WaitForSeconds(1f);
            currentRemainingTime = currentRemainingTime.Subtract(TimeSpan.FromSeconds(1));
            timer.text = currentRemainingTime.ToString(@"hh\:mm\:ss");
        }
        timer.text = currentRemainingTime.ToString(@"hh\:mm\:ss");
        CheckWheelToActive();
    }
    protected override void SetPopup()
    {
        SoundManager.instance.PlaySound("Button");
        if (!active)
        {
            this.animator.SetTrigger("Inactive");
            return;
        }
        gameScene.OpenPopup<WheelOfFortunePopup>(popup => popup.Initialize(SetTimerAfterSpin));  
    }

    private void SetTimerAfterSpin()
    {
        SetTimer();
        var nextTime = CheckWheelToActive();
        StartCoroutine(StartTimer(nextTime));
    }
    private void SetTimer()
    {
        var nextWheelActiveTime = DateTime.Now.AddHours(afterSpinWheelActiveNextTime);
        PlayerPrefs.SetString("wheel_of_fortune_timer",nextWheelActiveTime.ToBinary().ToString());
    }
}
