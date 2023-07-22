using System;
using System.Collections.Generic;
using UnityEngine;

public class LvlButtonsManager : MonoBehaviour
{
      [SerializeField] private List<LvlButton> lvlButtons;
      [SerializeField] private GameScene gameScene;
      private void Start()
      {
            var starsOfLvls = GameSystemsManager.instance.lvlSystem.lvlStars;
            for (int i = 0; i < starsOfLvls.Count; i++)
            {
                  lvlButtons[i].Initialize(true,i + 1,starsOfLvls[i],gameScene);
            }

            for (int i = starsOfLvls.Count; i < lvlButtons.Count; i++)
            {
                  lvlButtons[i].Initialize(false,i + 1,0,gameScene);
            }
      }
}
