using System;
using UnityEngine;

public class HealthSystem : IPublisher
{
    private int healthGameId;
    public event Action<int> OnItemCountChanged;
    private TimeSpan timeSpan;
    
    public HealthSystem(int gameId)
    {
        this.healthGameId = gameId;
        var initialHearths = GameSystemsManager.instance.gameProperties.initialHearths;
        var maxLives = GameSystemsManager.instance.gameProperties.maxLives;
        if (!PlayerPrefs.HasKey(gameId.ToString()))
        {
            PlayerPrefs.SetInt(gameId.ToString(),initialHearths);
            PlayerPrefs.SetInt("stored_lives",0);
        }
        var numLives = PlayerPrefs.GetInt(healthGameId.ToString());
        if (numLives < maxLives && PlayerPrefs.HasKey("next_hearth_time"))
        {
            CheckForNewHearth();
        }
    }

    public TimeSpan CheckForNewHearth()
    {
        var numLives = PlayerPrefs.GetInt(healthGameId.ToString());
        var maxLives = GameSystemsManager.instance.gameProperties.maxLives;
        if (numLives < maxLives && PlayerPrefs.HasKey("next_hearth_time")) 
        {
            var timeToNextHearth = GameSystemsManager.instance.gameProperties.nextHearthTime;
            var binaryDate = Convert.ToInt64(PlayerPrefs.GetString("next_hearth_time"));
            var prevNextLifeTime = DateTime.FromBinary(binaryDate);
            TimeSpan remainingTime;
            var now = DateTime.Now;
            if (prevNextLifeTime > now)
            {
                remainingTime = prevNextLifeTime - now;
                if (numLives < maxLives)
                {
                    SetTimeToNextLife((int) remainingTime.TotalSeconds);
                }
            }
            else
            {
                remainingTime = now - prevNextLifeTime;
                var livesToGive = ((int) remainingTime.TotalSeconds / timeToNextHearth) + 1;
                var newLivesCount = numLives + livesToGive;
                if (newLivesCount > maxLives)
                    livesToGive = maxLives - numLives;
                AddHearth(livesToGive);
                numLives += livesToGive;
                if(numLives < maxLives)
                {
                    SetTimeToNextLife(GameSystemsManager.instance.gameProperties.nextHearthTime);
                }
            }
            return remainingTime;
        }
        return TimeSpan.Zero;
    }

    public int GetHealthInfo()
    {
        return PlayerPrefs.GetInt(healthGameId.ToString());
    }

    public void RemoveHearth()
    {
        var numHealth = PlayerPrefs.GetInt(healthGameId.ToString());
        
       if(numHealth == GameSystemsManager.instance.gameProperties.maxLives)
            SetTimeToNextLife((int)GameSystemsManager.instance.gameProperties.maxLives);
        numHealth--;
        if (numHealth < 0)
            numHealth = 0;
        SetTimeToNextLife(GameSystemsManager.instance.gameProperties.nextHearthTime);
        PlayerPrefs.SetInt(healthGameId.ToString(), numHealth);
        OnItemCountChanged?.Invoke(-numHealth);
    }

    public void AddHearth(int count = 1)
    {
        var numHearth = PlayerPrefs.GetInt(healthGameId.ToString());
        numHearth += count;
        var maxLives = GameSystemsManager.instance.gameProperties.maxLives + GameSystemsManager.instance.gameProperties.maxStoredLives;
        if (numHearth > maxLives)
            numHearth = maxLives;
        PlayerPrefs.SetInt(healthGameId.ToString(),numHearth);
        OnItemCountChanged?.Invoke(numHearth);
    }
    public void Subscribe(Action<int> callback)
    {
        OnItemCountChanged += callback;
    }

    public void Unsubscribe(Action<int> callback)
    {
        if (OnItemCountChanged != null)
        {
            OnItemCountChanged -= callback;
        }
    }
    private void SetTimeToNextLife(int seconds)
    {
        var nextLifeTime = DateTime.Now.Add(TimeSpan.FromSeconds(seconds));
        PlayerPrefs.SetString("next_hearth_time", nextLifeTime.ToBinary().ToString());
    }
}
