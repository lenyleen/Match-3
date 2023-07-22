using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdvertisementSystem
{
    private readonly AdRequest adRequest;
    private readonly RewardedAd rewardedAd;
    private readonly InterstitialAd interstitialAd;
    private List<EventHandler<Reward>> rewardHandlers;
    private List<EventHandler<EventArgs>> interstitialHandlers;
    public AdvertisementSystem()
    {
        MobileAds.Initialize(Debug.Log);
        var rewardedAdId = GameSystemsManager.instance.gameProperties.rewardedAdId;
        var interstitialAdId = GameSystemsManager.instance.gameProperties.interstitialAdId;
        adRequest = new AdRequest.Builder().Build();
        rewardedAd = new RewardedAd(rewardedAdId);
        rewardHandlers = new List<EventHandler<Reward>>();
        interstitialHandlers = new List<EventHandler<EventArgs>>();
        interstitialAd = new InterstitialAd(interstitialAdId);
        rewardedAd.OnAdFailedToLoad += FailedToLoad;
        rewardedAd.OnAdFailedToShow += OnFailedToShow;
        rewardedAd.OnAdOpening += AdOpening;
        rewardHandlers.Add(UserEarnedReward);
        rewardedAd.OnUserEarnedReward += UserEarnedReward;
        interstitialAd.OnAdFailedToLoad += FailedToLoad;
        interstitialAd.OnAdFailedToShow += OnFailedToShow;
        interstitialAd.OnAdOpening += AdOpening;
        interstitialAd.OnAdClosed += OnAdClosed;
        interstitialAd.LoadAd(adRequest);
        rewardedAd.LoadAd(adRequest);
    }

    public void ShowRewardedAd(EventHandler<Reward> completeAction)
    {
        if (!rewardedAd.IsLoaded())
        {
            Debug.Log("Rewarded ad is not loaded");
            return;
        }
        rewardHandlers.Add(completeAction);
        rewardedAd.OnUserEarnedReward += completeAction;
        rewardedAd.Show();
        rewardedAd.LoadAd(adRequest);
    }

    public void ShowInterstitialAd(EventHandler<EventArgs> complete)
    {
        if (!interstitialAd.IsLoaded())
        {
            Debug.Log("InterstitialAd ad is not loaded");
            return;
        }
        interstitialHandlers.Add(complete);
        interstitialAd.OnAdClosed += complete;
        interstitialAd.Show();
        interstitialAd.LoadAd(adRequest);
    }

    private void RemoveCompleteActions<T>(List<T> eventHandlers, Action<T> unsubscription)
    {
        if(eventHandlers.Count <= 0) return;
        for (int i = 0; i < eventHandlers.Count; i++)
        {
            unsubscription?.Invoke(eventHandlers[i]);
            eventHandlers.Remove(eventHandlers[i]);
        }
    }
    private void UserEarnedReward(object sender, Reward args)
    {
        Debug.Log($"Item wit id {args.Type} was added in amount of {args.Amount}");
        RemoveCompleteActions(rewardHandlers, handler => rewardedAd.OnUserEarnedReward -= handler);
    }
    private void AdOpening(object sender, EventArgs msg)
    {
        Debug.Log($"Ad was closed");
    }

    private void OnAdClosed(object sender, EventArgs eventArgs)
    {
        Debug.Log("Interstitial Ad was shown");
        RemoveCompleteActions(interstitialHandlers, handler => interstitialAd.OnAdClosed -= handler);
    }

    private void OnFailedToShow(object sender, AdErrorEventArgs msg)
    {
        Debug.Log(msg.AdError);
        RemoveCompleteActions(rewardHandlers, handler => rewardedAd.OnUserEarnedReward -= handler);
        RemoveCompleteActions(interstitialHandlers, handler => interstitialAd.OnAdClosed -= handler);
    }
    private void FailedToLoad(object sender,AdFailedToLoadEventArgs msg)
    {
        Debug.Log($"Ad failed to lad wit execution error : {msg.LoadAdError}");
        RemoveCompleteActions(rewardHandlers,handler => rewardedAd.OnUserEarnedReward -= handler);
        RemoveCompleteActions(interstitialHandlers,handler => interstitialAd.OnAdClosed -= handler);
    }
}
