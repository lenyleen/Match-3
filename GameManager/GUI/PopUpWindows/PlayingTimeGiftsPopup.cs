using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public sealed class PlayingTimeGiftsPopup : LvlWindowPopUp
{
    [SerializeField] private List<StoreItem> possibleGifts;
    [SerializeField] private List<GiftForPlayingTime> imagesOfGifts;
    [SerializeField] private List<Text> timers;
    private int indexOfActiveTimer;
    private const int timeToGiftInMin = 15;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("game_day"))
        {
            PlayerPrefs.SetInt("game_day",DateTime.Now.Day);
            PlayerPrefs.SetInt("collected_gift_today",0);
            DateTime nextGiftTime = DateTime.Now.AddMinutes(timeToGiftInMin);
            PlayerPrefs.SetString("to_next_playing_time_gift_timer",nextGiftTime.ToBinary().ToString());
        }
        var gameDay = PlayerPrefs.GetInt("game_day");
        if (gameDay != DateTime.Now.Day)
        {
            ItemsDatabase.Instance.giftForPlayingTimeToday = new List<StoreItem>();
            PlayerPrefs.SetInt("game_day",DateTime.Now.Day);
            PlayerPrefs.SetInt("collected_gift_today", 0);
        }
        if (ItemsDatabase.Instance.giftForPlayingTimeToday.Count <= 0)
        {
            RewardCalculator calculator = new RewardCalculator(possibleGifts);
            ItemsDatabase.Instance.giftForPlayingTimeToday = calculator.CalculateDrop(100, 100, 100);
        }
        var gifts = ItemsDatabase.Instance.giftForPlayingTimeToday;
        var collectedGiftInfo  = PlayerPrefs.GetInt("collected_gift_today");
        indexOfActiveTimer = collectedGiftInfo;
        for (int i = 0; i < imagesOfGifts.Count; i++)
        {
            if(i > indexOfActiveTimer - 1)
            {
                imagesOfGifts[i].Initialize(gifts[i].sprite, gifts[i].GameID, 1, false);
                continue;
            }
            imagesOfGifts[i].Initialize(gifts[i].sprite, gifts[i].GameID, 1, true);
        }
        if(indexOfActiveTimer == imagesOfGifts.Count)
            return;
        var timerTime = CheckForGift();
        if(timerTime <= TimeSpan.Zero)
            return;
        StartCoroutine(StartTimer(timerTime));
    }
    private TimeSpan CheckForGift()
    {
        var giftTime = PlayerPrefs.GetString("to_next_playing_time_gift_timer");
        timers[indexOfActiveTimer].enabled = true;
        var now = DateTime.Now;
        var binaryDate = Convert.ToInt64(giftTime);
        var prevGiftTime = DateTime.FromBinary(binaryDate);
        var remainingTime = TimeSpan.Zero;
        if (prevGiftTime > now)
        {
            remainingTime = prevGiftTime - now;
        }
        else
        {
            timers[indexOfActiveTimer].enabled = false;
            imagesOfGifts[indexOfActiveTimer].Activate(OneOfGiftsWasCollected);
        }
        return remainingTime;
    }

    private void OneOfGiftsWasCollected()
    {
        indexOfActiveTimer++;
        if(indexOfActiveTimer >= timers.Count)
            return;
        PlayerPrefs.SetInt("collected_gift_today",indexOfActiveTimer + 1);
        var time = DateTime.Now.AddMinutes(timeToGiftInMin);
        PlayerPrefs.SetString("to_next_playing_time_gift_timer", time.ToBinary().ToString());
        var nexTime = CheckForGift();
        StopAllCoroutines();
        StartCoroutine(StartTimer(nexTime));
    }
    private IEnumerator StartTimer(TimeSpan currentRemainingTime)
    {
        timers[indexOfActiveTimer].enabled = true;   
        while (currentRemainingTime > TimeSpan.Zero)
        {
            yield return new WaitForSeconds(1f);
            currentRemainingTime = currentRemainingTime.Subtract(TimeSpan.FromSeconds(1));
            timers[indexOfActiveTimer].text = currentRemainingTime.ToString(@"mm\:ss");
        }
        timers[indexOfActiveTimer].text = currentRemainingTime.ToString(@"mm\:ss");
        CheckForGift();
    }
}
