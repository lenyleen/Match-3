using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class LvlSystem 
{
    public List<int> lvlStars { get; private set; }
    public LvlSystem()
    {
        if (!PlayerPrefs.HasKey("available_lvl"))
        {
            PlayerPrefs.SetInt("available_lvl",1);
        }
        if (!PlayerPrefs.HasKey("lvl_passed_stars"))
        {
            PlayerPrefs.SetString("lvl_passed_stars","0");
        }
        var starsHashed = PlayerPrefs.GetString("lvl_passed_stars");
        lvlStars = new List<int>();
        for (int i = 0; i < starsHashed.Length; i++)
        {
            var starsOfPassedLvl = (int)char.GetNumericValue(starsHashed,i);
            lvlStars.Add(starsOfPassedLvl);
        }
    }

    public void SetLvlAsPassed(int lvlNumber,int countOfStars, int score)
    {
        if(lvlStars.Count >= lvlNumber && lvlStars[lvlNumber - 1] >= countOfStars) return;
        GameSystemsManager.instance.scoreSystem.AddScore(score);
        lvlStars[lvlNumber - 1] = countOfStars;
        lvlStars.Add(0);
        string starsData = string.Join("", lvlStars);
        PlayerPrefs.SetInt("available_lvl", lvlNumber + 1);
        PlayerPrefs.SetString("lvl_passed_stars",starsData);
    }
}
