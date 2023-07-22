using UnityEngine;
public class GameProperties : ScriptableObject
{
     public int maxLives = 3;
     public int maxStoredLives = 3;
     public int initialCoins = 50;
     public int initialHearths = 3;
     public int extraMoves = 5;
     public int extraMovesCost = 50;
     public int totalScoreForFirstPlayerLvl = 25000;
     public int playerLvlIncreasePercent = 50;
     public int maxPlayerLvl = 60;
     public int nextHearthTime = 300;
     public string rewardedAdId = "ca-app-pub-3940256099942544/5224354917";
     public string interstitialAdId = "ca-app-pub-3940256099942544/1033173712";
}
