using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Task = System.Threading.Tasks.Task;

public class CellGridFiller
{
    private CellGrid cellGrid;
    public Action<bool> fillingСompleted;
    private readonly Dictionary<Cell,Cell> cellsInFrontAIndestructible;
    private readonly float distanceBetweenCells;
    private float lastCandyPosition;
    private readonly float yFirstToFillCandyPosition;

    public CellGridFiller(CellGrid cellGrid)
    {
        this.cellGrid = cellGrid;
        cellsInFrontAIndestructible = FindCellsInFrontAIndestructible(cellGrid);
        distanceBetweenCells = cellGrid.DistanceBetweenCells;
        lastCandyPosition = cellGrid.LastCandyYPosition;
        yFirstToFillCandyPosition = lastCandyPosition + (distanceBetweenCells * (cellGrid.GetLength(1)));
    }
    private Dictionary<Cell,Cell> FindCellsInFrontAIndestructible(CellGrid cellGrid)
    {
        var holeyCells = new Dictionary<Cell,Cell>();
        for (int column = cellGrid.GetLength(0) - 1; column >= 0; column--)
        {
            for (int row = cellGrid.GetLength(1) - 2; row >= 0; row--)
            {
                if (cellGrid[column, row] != null || cellGrid[column, row + 1] == null) continue;
                if(column > 0 && cellGrid[column - 1, row] != null)
                {
                    holeyCells.Add(cellGrid[column, row + 1], cellGrid[column - 1, row]);
                    continue;
                }
                holeyCells.Add(cellGrid[column,row + 1], cellGrid[column + 1, row]);
            }
        }
        return holeyCells;
    }
       
    public async Task FillEmptyCells()
    {
        var candies = AssignCandies();
        if (candies.Count == 0)
        {   
            fillingСompleted?.Invoke(false);
            return;
        }
        while (candies.Count != 0)
        {
            await FillCellls(candies);
            candies = AssignCandies();
        }
        await Task.Delay(300);
        fillingСompleted?.Invoke(true);
    }

    private async Task FillCellls(List<Candy> assignedCandies)
    {
        await Task.WhenAll(assignedCandies.Select(s => s.GoToYourCell(Constants.Speed)));
        SoundManager.instance.PlaySound("CandyFalling");
    } 
    
     private Candy SpawnCandyToFillCells(int column, int row, float yCandyPosition)
    {
        var newCandyPosition = new Vector2(cellGrid[column, row].transform.position.x,
            yCandyPosition);
        var newCandy = cellGrid.SpawnRandomCandy(column,row);
        newCandy.transform.position = newCandyPosition;
        return newCandy;
    }
    private  List<Candy> AssignCandies()
    {
        List<Candy> newPositionedCandies = new List<Candy>();
        for (int column = cellGrid.GetLength(0) - 1; column >= 0; column--)
        {
            var yCandyPosition = yFirstToFillCandyPosition;
            for (int row = cellGrid.GetLength(1) - 1; row >= 0; row--)
            { 
                if(cellGrid[column,row] == null) continue;
                if(cellGrid[column,row].Candy != null) continue;
                var emptyCell = cellGrid[column, row];
                for (int _row = row; _row >= 0; _row--)
                {
                    if(cellGrid[column,_row] == null) break;
                    if(cellGrid[column,_row].Candy)
                    {
                        emptyCell.Assign(cellGrid[column, _row].Candy);
                        emptyCell.Candy.AssignCell(emptyCell);
                        cellGrid[column, _row].Candy = null;
                        newPositionedCandies.Add(emptyCell.Candy);
                        break;
                    }
                }
                if(cellGrid[column,row].Candy == null && CheckForNullCellInColumn(column,row))
                {
                    var newCandy = SpawnCandyToFillCells(column, row, yCandyPosition);
                    emptyCell.Assign(newCandy);
                    newCandy.AssignCell(emptyCell);
                    newPositionedCandies.Add(newCandy);
                    yCandyPosition += distanceBetweenCells;
                }
            }
        }
        foreach (var cells in cellsInFrontAIndestructible)
        {
            if (cells.Key.Candy != null) continue;
            var emptyCell = cellsInFrontAIndestructible[cells.Key];
            if(emptyCell.Candy == null) continue;
            emptyCell.Candy.AssignCell(cells.Key);
            cells.Key.Assign(emptyCell.Candy);
            emptyCell.Candy = null;
            newPositionedCandies.Add(cells.Key.Candy);
        }
        return newPositionedCandies;
    }
    private bool CheckForNullCellInColumn(int column,int row)
    {
        for (int _row = row; _row >= 0; _row--)
        {
            if (cellGrid[column, _row] == null)
                return false;
        }
        return true;
    }
}
