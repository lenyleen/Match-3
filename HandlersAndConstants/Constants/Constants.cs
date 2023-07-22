
using System.Collections.Generic;
using UnityEngine;


public static class Constants
{
    public const int Rows = 9;
    public const int Columns = 8;
    public const float AnimationDuration =  0.2f;
    public const float DistanceBetweenCells = 1.8f;
    public static readonly Vector2 CellGridWorldPosition = new Vector3(-6.3f,7.4f,0);
    public static readonly Vector2 CellSize = new Vector2(Constants.DistanceBetweenCells, Constants.DistanceBetweenCells);
    public const float LastCellYPosition = -5f;
    public const  float YCandyPositionToFill = LastCellYPosition + (Rows * DistanceBetweenCells);
    public static readonly int[] ChancesOfDrop = new[] {100, 50, 15};
    public static readonly List<Vector3> BezierAnchorsPositions = new List<Vector3>()
    {
        new Vector3(-7f,14f,0),
        new Vector3(-6f,14f,0),
        new Vector3(-5f, 14f, 0)
    };
    
    public const float StandardSizeOfGUIImage = 86f;

    public const float MoveAnimationMinDuration = 0.05f;
    public const float Speed = 25f;
    public const float SwitchCandySpeed = 28f;
    public const float StandardCircleCastRadius = 1.5f;
    public const int StandardCandiesParticleEffectDelay = 200;

    public const  int CandiesInPoolCount = 32;

    public const string ApplicationId = "ca-app-pub-4861481139058515~6351297148";
    public const string InterstitialAdId = "ca-app-pub-3940256099942544/1033173712";
    public const string RewardedAdId = "ca-app-pub-3940256099942544/5224354917";
}
