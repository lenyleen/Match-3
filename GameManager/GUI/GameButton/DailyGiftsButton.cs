using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class DailyGiftsButton : GameButton
{
    [SerializeField] private Text timer;
    [SerializeField] private bool giftAvailable;
    private int afterGiftTakenAvailableNextTime = 24;
    protected override void Start()
    {
        if(!PlayerPrefs.HasKey("daily_gifts_timer"))
            PlayerPrefs.SetString("daily_gifts_timer",DateTime.Now.ToBinary().ToString());
        var timerTime = CheckDailyGiftsToActive();
        StartCoroutine(StartTimer(timerTime));
        button.onClick.AddListener(SetPopup);
    }
    private TimeSpan CheckDailyGiftsToActive()
    {
        var dailyGiftActiveNextTime = PlayerPrefs.GetString("daily_gifts_timer");
        var binaryDate = Convert.ToInt64(dailyGiftActiveNextTime);
        var prevNextGiftTime = DateTime.FromBinary(binaryDate);
        TimeSpan remainingTime = TimeSpan.Zero;
        var now = DateTime.Now;
        if (prevNextGiftTime > now)
        {
            giftAvailable = false;
            remainingTime = prevNextGiftTime - now;
        }
        else
        {
            giftAvailable = true;
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
        CheckDailyGiftsToActive();
    }

    private void SetTimer()
    {
        var nextGiftAvailableNexTime = DateTime.Now.AddHours(afterGiftTakenAvailableNextTime);
        PlayerPrefs.SetString("daily_gifts_timer",nextGiftAvailableNexTime.ToBinary().ToString());
        var remainingTime = CheckDailyGiftsToActive();
        StartCoroutine(StartTimer(remainingTime));
    }
    protected override void SetPopup()
    {
        SoundManager.instance.PlaySound("Button");
        gameScene.OpenPopup<DailyGiftsPopUp>(popup => popup.Initialize(SetTimer, giftAvailable));
    }
}
