using UnityEngine;

public class ScoreSystem
{
      public int totalScore;
      public int toNextLvlTotalScore;
      public int playerLvl;

      public ScoreSystem()
      {
            if(!PlayerPrefs.HasKey("player_total_score"))
                  PlayerPrefs.SetInt("player_total_score",0);
            if (!PlayerPrefs.HasKey("player_lvl"))
            {
                  PlayerPrefs.SetInt("player_lvl", 0);
                  PlayerPrefs.SetInt("to_next_lvl_total_score",
                        GameSystemsManager.instance.gameProperties.totalScoreForFirstPlayerLvl);
                  toNextLvlTotalScore = GameSystemsManager.instance.gameProperties.totalScoreForFirstPlayerLvl;
            }
            totalScore = PlayerPrefs.GetInt("player_total_score");
            toNextLvlTotalScore = PlayerPrefs.GetInt("to_next_lvl_total_score");
            playerLvl = PlayerPrefs.GetInt("player_lvl");
      }

      public void AddScore(int count)
      {
            totalScore += count;
            PlayerPrefs.SetInt("player_total_score", totalScore);
            CheckForNewPlayerLvl();
      }
      private void CheckForNewPlayerLvl()
      {
            var lvlsToAdd = 0;
            while (totalScore > toNextLvlTotalScore)
            {
                  var percentToNumber = (toNextLvlTotalScore * GameSystemsManager.instance.gameProperties.playerLvlIncreasePercent) / 100;
                  toNextLvlTotalScore += percentToNumber;
                  lvlsToAdd++;
            }
            PlayerPrefs.SetInt("to_next_lvl_total_score",toNextLvlTotalScore);
            playerLvl += lvlsToAdd;
            PlayerPrefs.SetInt("player_lvl",playerLvl);
      }
}
