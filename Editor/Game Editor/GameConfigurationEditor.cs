using System;
using UnityEditor;
using UnityEngine;

public class GameConfigurationEditor : IGameEditorWindow
{
    public string Name => "Game Configuration";
    private readonly GameProperties gameProperties;
    public GameConfigurationEditor()
    {
        var data = Resources.LoadAll<GameProperties>("CandiesGridData/");
        if(data.Length > 1)
            Debug.LogWarning("More than one configuration found!");
        if (data.Length <= 0)
        {
            gameProperties = ScriptableObject.CreateInstance<GameProperties>();
            AssetDatabase.CreateAsset(gameProperties, $"Assets/Resources/CandiesGridData/GameConfiguration.asset");
            return;
        }
        gameProperties = data[0]; 
    }

    public void DrawWindow()
    {
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height  - 700, 250,Screen.height),new GUIStyle());
        EditorGUILayout.LabelField("GAME PROPERTIES EDITOR",EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Health system properties :",EditorStyles.boldLabel);
        gameProperties.maxLives = EditorGUILayout.IntField("Max lives property",gameProperties.maxLives);
        gameProperties.maxStoredLives = EditorGUILayout.IntField("Max stored lives",gameProperties.maxStoredLives);
        gameProperties.initialHearths = EditorGUILayout.IntField("Initializing players hearths",gameProperties.initialHearths);
        gameProperties.nextHearthTime = EditorGUILayout.IntField("Next free hearth time",gameProperties.nextHearthTime);
        EditorGUILayout.LabelField("Coins system properties :",EditorStyles.boldLabel);
        gameProperties.initialCoins = EditorGUILayout.IntField("Initializing players coins",gameProperties.initialCoins);
        gameProperties.extraMovesCost = EditorGUILayout.IntField("Cost of extra moves",gameProperties.extraMovesCost);
        EditorGUILayout.LabelField("Lvl system properties :",EditorStyles.boldLabel);
        gameProperties.playerLvlIncreasePercent = EditorGUILayout.IntField("Increase percentage for the next level %",gameProperties.playerLvlIncreasePercent);
        gameProperties.totalScoreForFirstPlayerLvl = EditorGUILayout.IntField("Total score to reach the first level",gameProperties.totalScoreForFirstPlayerLvl);
        gameProperties.maxPlayerLvl = EditorGUILayout.IntField("Max level a player can reach",gameProperties.maxPlayerLvl);
        gameProperties.extraMoves = EditorGUILayout.IntField("Added moves after AD or purchase",gameProperties.extraMoves);
        EditorGUILayout.LabelField("Advertisement system properties:",EditorStyles.boldLabel);
        gameProperties.interstitialAdId = EditorGUILayout.TextField("Interstitial Ad ID",
            gameProperties.interstitialAdId);
        gameProperties.rewardedAdId = EditorGUILayout.TextField("Rewarded Ad ID",
            gameProperties.rewardedAdId);
        GUILayout.EndArea();
    }
}
