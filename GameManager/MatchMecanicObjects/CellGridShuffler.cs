
using System;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

public static class CellGridShuffler
{
      public static event Action onShuffleStarted;
      public static async Task ShuffleCellGrid(CellGrid cellGrid)
      {
            onShuffleStarted?.Invoke();
            await Task.Delay(Constants.StandardCandiesParticleEffectDelay);
            for (int column =  0; column < Constants.Columns; column++)
            {
                  for (int row = 0; row < Constants.Rows; row++)
                  {
                        var randomColumn = Random.Range(0, Constants.Columns - 1);
                        var randomRow = Random.Range(0, Constants.Rows - 1);
                        if(cellGrid[column,row] == null || cellGrid[randomColumn,randomRow] == null) continue;
                        cellGrid[column,row].Candy.ReassignCell(cellGrid[randomColumn,randomRow],cellGrid[column,row],cellGrid[randomColumn,randomRow].Candy);
                        await Task.WhenAll(cellGrid[column, row].Candy.GoToYourCell(Constants.Speed * 1.5f),
                              cellGrid[randomColumn,randomRow].Candy.GoToYourCell(Constants.Speed * 1.5f));
                  }
            }
            
      }
}
