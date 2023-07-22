using System;
using System.Threading.Tasks;
using UnityEngine;


public class OnLvlEndBonusCreator
{
       private CellGrid cellGrid;
       private CellGridFiller gridFiller;
       public event Action onBonusTimeComplete;
       public OnLvlEndBonusCreator(CellGrid cellGrid, CellGridFiller gridFiller)
       {
              this.cellGrid = cellGrid;
              this.gridFiller = gridFiller;
       }

       public async Task StartBonusTime(int movesCount)
       {
              await Task.Delay(1000);
              for (int i = 0; i < movesCount; i++)
              {
                     await SpawnBonusCandy();
                     await gridFiller.FillEmptyCells();
                     EventBus.RaiseEvent<IMoveEndedObserver>(listener => listener.MoveEnded());
              }
              onBonusTimeComplete?.Invoke();
       }
       private async Task SpawnBonusCandy()
       {
              await Task.Delay(1000);
              var bonusCandy = cellGrid.SpawnRandomBonusCandyWithRandomCell();
              await Task.Delay(300);
              await bonusCandy.DoCellGridMemberAction();
       }
}
