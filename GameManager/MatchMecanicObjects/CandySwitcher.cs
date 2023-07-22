using System;
using System.Threading.Tasks;

public class CandySwitcher
{
     private readonly MatchFinder matchFinder;
     private readonly SwitchedBonusCandiesHandler bonusHandler;
     private readonly CellGridFiller cellGridFiller;
     public CandySwitcher(MatchFinder matchFinder, CellGridFiller cellGridFiller)
     {
          bonusHandler = new SwitchedBonusCandiesHandler(cellGridFiller);
          this.matchFinder = matchFinder;
          this.cellGridFiller = cellGridFiller;
     }
     public async void  SwitchPositions(Candy firstHitCandy, Candy secondHitCandy)
     {
          EventBus.RaiseEvent<IGamePause>(input => input.Pause(this,true));
          firstHitCandy.ReassignCell(secondHitCandy.cell,firstHitCandy.cell,secondHitCandy);
          await Task.WhenAll(secondHitCandy.GoToYourCell(Constants.SwitchCandySpeed),
               firstHitCandy.GoToYourCell(Constants.SwitchCandySpeed));
          var bonusDefineResult =  await bonusHandler.DefineBonusMatch(secondHitCandy, firstHitCandy);
          if (bonusDefineResult)
          {
               await CheckAfterShuffleOrBonus();
               EventBus.RaiseEvent<IMoveEndedObserver>(observer => observer.MoveEnded());
               return;
          }
          var matchFindingResult = await matchFinder.TryToFindCandyMatch(firstHitCandy.cell, secondHitCandy.cell);
          if (matchFindingResult)
          {
               EventBus.RaiseEvent<IMoveEndedObserver>(observer => observer.MoveEnded());
               EventBus.RaiseEvent<IGamePause>(input => input.Pause(this,false));
               return;
          }
          UndoSwitch(firstHitCandy,secondHitCandy);
     }

     public async Task CheckAfterShuffleOrBonus()
     {
          await cellGridFiller.FillEmptyCells();
          await matchFinder.TryToFindCandyMatch(null, null);
          EventBus.RaiseEvent<IGamePause>(input => input.Pause(this,false));
     }
     
     private async void UndoSwitch(Candy firstHitCandy, Candy secondHitCandy)
     {
          firstHitCandy.ReassignCell(secondHitCandy.cell,firstHitCandy.cell,secondHitCandy);
          await Task.WhenAll(firstHitCandy.GoToYourCell(Constants.SwitchCandySpeed),
               secondHitCandy.GoToYourCell(Constants.SwitchCandySpeed));
          EventBus.RaiseEvent<IGamePause>(input => input.Pause(this,false));
     }

     
}