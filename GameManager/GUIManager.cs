using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private CollectiblesWindow collectiblesWindow;
    [SerializeField] private MovesCounter movesCounter;
    [SerializeField] private ScoreCounter scoreCounter;
    [SerializeField] private AppearingWord appearingWord;
    [SerializeField] private GameScene gameScene;
    [SerializeField] private Text lvlNumber;
    [SerializeField] private InGameTimer timer;
    private OnLvlEndBonusCreator bonusTimeHandler;
    private IGameMovesLimiter movesLimiter;
    private int numberOfLvl;
    private bool scoreAsGoal;
    public void Initialize(CandiesGridDataHolder data,ValueOfItemsDataHolder valueOfItemsData, OnLvlEndBonusCreator bonusTimeHandler)
    {
        this.numberOfLvl = data.numberOfLevel;
        this.lvlNumber.text = $"Level {data.numberOfLevel}";
        collectiblesWindow.Initialize(data.dataOfCollectiblesAsGoal,data.scoreGoal,scoreCounter);
        appearingWord.Initialize();
        this.bonusTimeHandler = bonusTimeHandler;
        bonusTimeHandler.onBonusTimeComplete += LvlComplete;
        collectiblesWindow.onScoreReached += GoalReached;
        var scoreOfItems = valueOfItemsData.valueOfItems.ToDictionary(element => element._obj, element => element.count);
        scoreCounter.Initialize(data.maxLvlScore,data.percentsOfScoreStars,scoreOfItems);
        scoreAsGoal = data.scoreAsGoal;
        if (scoreAsGoal)
            movesLimiter = timer;
        else
            movesLimiter = movesCounter;
        movesLimiter.Initialize(data.lvlLimitCounterNumber);
        movesLimiter.onOutOfLimit += OutOfMoves;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
          LvlComplete();  
    }

    private async void GoalReached()
    {
        movesCounter.onOutOfLimit -= OutOfMoves;
        collectiblesWindow.onScoreReached -= GoalReached;
        EventBus.RaiseEvent<IGamePause>(input => input.Pause(this,true));
        if (scoreAsGoal)
        {
            LvlComplete();
            return;
        }
        EventBus.RaiseEvent<ILvlCompletedObserver>(observer => observer.OnGoalReached());
        await bonusTimeHandler.StartBonusTime(movesCounter.CurrentNumberOfMoves);
    }

    private void LvlComplete()
    {
        RewardCalculator calculator = new RewardCalculator(ItemsDatabase.Instance.allItems);
        var reward = calculator.CalculateDrop(Constants.ChancesOfDrop);
        gameScene.OpenPopup<LvlCompleteWindowPopUp>(popup => popup.Initialize((int)scoreCounter.CompleteScore,scoreCounter.CompleteCountOfStars,numberOfLvl,reward));
    }
    private void OutOfMoves()
    {
        gameScene.OpenPopup<OutOfMovesWindowPopUp>(popup => popup.Initialize(scoreAsGoal,PaymentResult));
    }
    private void PaymentResult(bool result)
    {
        if(!result)
        {
            gameScene.OpenPopup<LvlFailedPopUp>(popup => popup.Initialize((int)scoreCounter.CompleteScore, scoreCounter.CompleteCountOfStars, numberOfLvl));
            GameSystemsManager.instance.healthSystem.RemoveHearth();
            return;
        }
        movesCounter.AddMovesAfterPayment();
    }
    private void OnDisable()
    {
        collectiblesWindow.onScoreReached -= GoalReached;
        movesCounter.onOutOfLimit -= OutOfMoves;
        bonusTimeHandler.onBonusTimeComplete -= LvlComplete;
    }
}
