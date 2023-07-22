using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

public class MatchFinder
{
    private readonly CellGridFiller cellGridFiller;
    private readonly CellGrid cellGrid;

    public MatchFinder(CellGridFiller cellGridFiller, CellGrid cellGrid)
    {
        this.cellGrid = cellGrid;
        this.cellGridFiller = cellGridFiller;
    }
    public async Task<bool> TryToFindCandyMatch(Cell hitColliderCell, Cell candyCell)
    {
        var match = FindMatch(hitColliderCell,candyCell);
        if (match == null || match.Count <= 0) return false;
        while (match.Count > 0)
        {
            await Task.WhenAll(match.Select(match => match.DoAnArrayAction()));
            await Task.Delay(300);
            await cellGridFiller.FillEmptyCells();
            match = FindMatch(hitColliderCell,candyCell);
        }
        return true;
    }

    private List<MatchedCandiesArray> FindMatch(Cell hitColliderCell,Cell candyCell)
    {
        var columnCount = cellGrid.GetLength(0);
        var rowCount = cellGrid.GetLength(1);
        Dictionary<int, MatchedCandiesArray> matches = new Dictionary<int, MatchedCandiesArray>();
        Cell switchedCell = null;
        for (int column = 0; column < cellGrid.GetLength(0); column++)
        {
            for (int row = 0; row < cellGrid.GetLength(1); row++)
            {
                var currentCell = cellGrid[column, row];
                if (currentCell == null || currentCell.Candy == null) continue;
                switchedCell = currentCell == hitColliderCell || currentCell == hitColliderCell ? currentCell : null;
                var horMatch = FindHorMatch(column, row, columnCount, currentCell);
                var verMatch = FindVertMatch(column, row, rowCount, currentCell);
                var squareMatch = SquareMatch(column, row,columnCount,rowCount,currentCell );
                if (horMatch == null && verMatch == null && squareMatch == null) continue;
                if (!matches.ContainsKey(currentCell.matchId))
                    matches.Add(currentCell.matchId,
                        new MatchedCandiesArray(new HashSet<Candy>(),switchedCell));
                matches[currentCell.matchId].AddMoreMatchedCells(horMatch, PositionInSpace.Horizontal,switchedCell);
                matches[currentCell.matchId].AddMoreMatchedCells(verMatch, PositionInSpace.Vertical,switchedCell);
                matches[currentCell.matchId].AddMoreMatchedCells(squareMatch,PositionInSpace.Mixed,switchedCell);
            }
        }
        SetCellsIdByDefault();
        return matches.Values.Where(match => match.Count > 2).ToList();
    }

    private bool CompareCandies(Cell currentCell, Cell cellToCompare)
    {
        var result = cellToCompare != null && cellToCompare.Candy != null &&
                     cellToCompare.Candy.Equals(currentCell.Candy);
        if (!result) return false;
        if (cellToCompare.Matched())
        {
            currentCell.matchId = cellToCompare.matchId;
            return true;
        }
        cellToCompare.matchId = currentCell.matchId;
        return true;
    }

    private List<Candy> FindHorMatch(int column, int row, int columnCount,Cell current)
    {
        List<Candy> result = new List<Candy>() {current.Candy};
        for (int i = column + 1; i < columnCount; i++)
        {
            var compare = CompareCandies(current, cellGrid[i, row]);
            if(!compare) break;
            result.Add(cellGrid[i, row].Candy);
        }
        return result.Count >= 3 ? result : null;
    }
    private List<Candy> FindVertMatch(int column, int row, int rowCount, Cell current)
    {
        List<Candy> result = new List<Candy>() {current.Candy};
        for (int i = row + 1; i < rowCount; i++)
        {
            var compare = CompareCandies(current, cellGrid[column, i]);
            if(!compare) break;
            result.Add(cellGrid[column,i].Candy);
        }
        return result.Count >= 3 ? result : null;
    }
    private  List<Candy> SquareMatch(int column, int row,int columnCount, int rowCount, Cell currentCell)
    {
        if (row >= rowCount - 1) return null;
        if (column >= columnCount - 1) return null;
        if (!CompareCandies(currentCell, cellGrid[column + 1, row + 1])) return null;
        if (!CompareCandies(currentCell, cellGrid[column, row + 1])) return null;
        if (!CompareCandies(currentCell, cellGrid[column + 1, row])) return null;
        return new List<Candy>(){currentCell.Candy,cellGrid[column + 1, row + 1].Candy,cellGrid[column, row + 1].Candy,cellGrid[column + 1, row].Candy};
    }

    private void SetCellsIdByDefault()
    {
        for (int column = 0; column < cellGrid.GetLength(0); column++)
        {
            for (int row = 0; row < cellGrid.GetLength(1); row++)
            {
                if(cellGrid[column,row] != null)
                    cellGrid[column,row].SetIdByDefault();
            }
        }
    }
}
