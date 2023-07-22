using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SuggestedMatchFinder
{
        private readonly CellGrid cellGrid; 
        public async void PotentialMatches()
        {
            List<Cell> matches = new List<Cell> ();
            for (int row = 0; row < Constants.Rows; row++)
            {
                for (int column = 0; column < Constants.Columns; column++)
                {
                    if(cellGrid[column,row] == null) continue;
                    var horizontalMatch = HorizontalMatches1(column, row);
                    var verticalMatches = VerticalMatches(column, row);
                    if(horizontalMatch != null)
                        matches.Add(horizontalMatch);
                    if(verticalMatches != null)
                        matches.Add(verticalMatches);
                }
            }
            if (matches.Count <= 0)
            {
                EventBus.RaiseEvent<IGamePause>(input => input.Pause(this,true));
                await CellGridShuffler.ShuffleCellGrid(cellGrid);
                EventBus.RaiseEvent<IGamePause>(input => input.Pause(this,false));
                return;
            }
            var randomNumber = Random.Range(0, matches.Count);
            matches[randomNumber].Candy.PlayAnimation(CandyAnimationTriggers.SuggestedMatch);
        }
        public SuggestedMatchFinder(CellGrid cellGrid)
        {
            this.cellGrid = cellGrid;
        }

        private bool CheckCandiesToEquality(Cell currentCell, Cell cellForCheck)
        {
            return cellForCheck != null && currentCell.Candy.Equals(cellForCheck.Candy);
        }

        private Cell VerticalMatches(int column, int row)
        {
            if (row < 1 || row > Constants.Rows - 2) return null;
            var currentCell = cellGrid[column, row];
            if (column <= Constants.Columns - 2)
            {
                if (CheckCandiesToEquality(currentCell, cellGrid[column + 1, row - 1]))
                {
                    if (CheckCandiesToEquality(currentCell,cellGrid[column, row + 1]))
                        return cellGrid[column + 1, row - 1];
                    /* example *\
                     * * * * *
                     * * * * *
                     * * & * *
                     * $ * * *
                     * & * * *
                    \* example  */
                    if (CheckCandiesToEquality(currentCell, cellGrid[column + 1, row + 1]))
                        return currentCell;
                    /* example *\
                     * * * * *
                    * * * * *
                     * & * * *
                     $ * * * *
                     * & * * *
                    \* example  */
                }

                if (!CheckCandiesToEquality(currentCell,cellGrid[column, row - 1]))
                    return null;
                if (CheckCandiesToEquality(currentCell,cellGrid[column + 1, row + 1]))
                    return cellGrid[column + 1, row + 1];
                /* example *\
                 * * * * *
                 * * * * *
                 * & * * *
                 * $ * * *
                 * * & * *
                \* example  */
            }

            if (column < 1) return null;
            if (CheckCandiesToEquality(currentCell,cellGrid[column - 1, row + 1]))
            {
                if (CheckCandiesToEquality(currentCell, cellGrid[column, row - 1]))
                    return cellGrid[column - 1, row + 1];
                /* example *\
                 * * * * *
                 * * * * *
                 * & * * *
                 * $ * * *
                 & * * * *
                \* example  */
                if (CheckCandiesToEquality(currentCell, cellGrid[column - 1, row - 1]))
                    return currentCell;
                /* example *\
                 * * * * *
                 * * * * *
                 & * * * *
                 * $ * * *
                 & * * * *
                \* example  */
            }

            if (!CheckCandiesToEquality(currentCell, cellGrid[column - 1, row - 1])) return null;
            if (CheckCandiesToEquality(currentCell, cellGrid[column, row + 1]))
                return cellGrid[column - 1, row - 1];
            /* example *\
             * * * * *
             * * * * *
             & * * * *
             * $ * * *
             * & * * *
            \* example  */
            return null;
        }

    private Cell HorizontalMatches1(int column, int row)
    {
        if (column < 1 || column > Constants.Columns - 2) return null;
        var currentCell = cellGrid[column, row];
        if (row >= 1)
        {
            if (CheckCandiesToEquality(currentCell,cellGrid[column - 1, row - 1]))
            {
                if (CheckCandiesToEquality(currentCell, cellGrid[column + 1, row]))
                    return cellGrid[column - 1, row - 1];
                /*example*\
                 * * * * *
                 * * * * *
                 & * * * *
                 * $ & * *
                 * * * * *
                \*example*/
                if (CheckCandiesToEquality(currentCell, cellGrid[column + 1, row - 1]))
                    return currentCell;
                        
                    
                /*example*\
                 * * * * *
                 * * * * *
                 * * * * *
                 & * & * *
                 * $ * * *
                \*example*/
            }

            if (CheckCandiesToEquality(currentCell,cellGrid[column - 1, row]))
            {
                if (CheckCandiesToEquality(currentCell, cellGrid[column + 1, row - 1]))
                    return cellGrid[column + 1, row - 1];
            }
            /*example*\
             * * * * *
             * * * * *
             * * * * *
             * * & * *
             & $ * * *
            \*example*/
        }

        if (row <= Constants.Rows - 2)
        {
            if (CheckCandiesToEquality(currentCell,cellGrid[column + 1, row + 1]))
            {
                if (CheckCandiesToEquality(currentCell,cellGrid[column - 1, row + 1]))
                    return currentCell;
                /*example*\
                 * * * * *
                 * * * * *
                 * $ * * *
                 & * & * *
                 * * * * *
                \*example*/
                if (CheckCandiesToEquality(currentCell,cellGrid[column - 1, row]))
                    return cellGrid[column + 1, row + 1];
                /*example*\
                 * * * * *
                 * * * * *
                 & $ * * *
                 * * & * *
                 * * * * *
                \*example*/
            }

            if (!CheckCandiesToEquality(currentCell,cellGrid[column + 1, row])) return null;
            if (CheckCandiesToEquality(currentCell,cellGrid[column - 1, row + 1]))
                return cellGrid[column - 1, row + 1];
            /*example*\
             * * * * *
             * * * * *
             * $ & * *
             & * * * *
             * * * * *
            \*example*/
        }
        return null;
    }
}
