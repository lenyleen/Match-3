using System;
using System.Collections.Generic;
using UnityEngine;

public class CandiesGridDataHolder : ScriptableObject
{
    public int lvlLimitCounterNumber;
    public List<Element<GameObject>> candiesData;
    public List<Element<GameObject>> blockersData;
    public List<Element<GameObject>> collectiblesDataToSpawn;
    public List<Element<Texture>> dataOfCollectiblesAsGoal;
    public int numberOfLevel;
    public List<int> percentsOfScoreStars;
    public int scoreGoal;
    public int maxLvlScore;
    public bool scoreAsGoal;

    public void Initialize(int lvlLimitCounterNumber, int numberOfLevel, List<Element<GameObject>> candiesTexturesNames,
        List<Element<GameObject>> blockersTexturesNames,
        List<Element<GameObject>> collectablesTexturesNames, List<Element<Texture>> dataOfCollectiblesAsGoal,
        int scoreGoal, List<int> starsPercents, int maxLvlScore,bool scoreAsGoal)
    {
        this.maxLvlScore = maxLvlScore;
        this.scoreGoal = scoreGoal;
        percentsOfScoreStars = starsPercents;
        this.candiesData = candiesTexturesNames;
        this.blockersData = blockersTexturesNames;
        this.collectiblesDataToSpawn = collectablesTexturesNames;
        this.numberOfLevel = numberOfLevel;
        this.dataOfCollectiblesAsGoal = dataOfCollectiblesAsGoal;
        this.lvlLimitCounterNumber = lvlLimitCounterNumber;
        this.scoreAsGoal = scoreAsGoal;
    }
}
