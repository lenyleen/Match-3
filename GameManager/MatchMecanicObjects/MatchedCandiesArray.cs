using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MatchedCandiesArray
{
    private readonly HashSet<Candy> matchedCandies;
    private CandyColor Type { get; set; }
    public int Count { get; private set; }
    private PositionInSpace PositionInSpace { get; set; }
    private Cell SwitchedCandyCell { get; set; }
    private Func<Task> bonusCandyDeterminant;
    public MatchedCandiesArray(HashSet<Candy> matchedCandies, Cell switchedCandyCell)
    {
        SwitchedCandyCell = switchedCandyCell;
        this.matchedCandies = matchedCandies;
        this.PositionInSpace = PositionInSpace.None;
    }

    public void AddMoreMatchedCells(List<Candy> cells,PositionInSpace positionInSpace,Cell switchedCandyCell)
    {
        if(cells == null) return;
        matchedCandies.UnionWith(cells);
        if (PositionInSpace == PositionInSpace.None)
        {
            this.PositionInSpace = positionInSpace;
        }
        else if (positionInSpace != this.PositionInSpace)
        {
            this.PositionInSpace = PositionInSpace.Mixed;
        }
        Type = matchedCandies.First().Color;
        this.SwitchedCandyCell = switchedCandyCell != null ? switchedCandyCell : SwitchedCandyCell;
        Count = matchedCandies.Count > 5 ? 5 : matchedCandies.Count;
        bonusCandyDeterminant = Count switch
        {
            3 => DoThreeCandiesAction,
            4 => DoFourCandiesAction,
            5 => DoFiveCandiesAction,
            _ => null
        };
            
    }
    public async Task DoAnArrayAction()
    {
        if (SwitchedCandyCell == null)
            SwitchedCandyCell =  matchedCandies.First().cell;
        await bonusCandyDeterminant?.Invoke()!;
    }
    private  async Task DoThreeCandiesAction()
    {
        SoundManager.instance.PlaySound("CandyMatch");
        await Task.WhenAll(matchedCandies.Select(candy => candy.DoCellGridMemberAction()));
    }

    private async Task DoFourCandiesAction()
    {
        TypeOfCandyBonus typeOfCandyBonus =  TypeOfCandyBonus.BombCandy;
        if (PositionInSpace != PositionInSpace.Mixed)
            typeOfCandyBonus = PositionInSpace == PositionInSpace.Horizontal ? TypeOfCandyBonus.StripedHorizontalCandy : TypeOfCandyBonus.StripedVerticalCandy;
        await DoBonusArrayAction();
        SwitchedCandyCell.SpawnBonusCandy(typeOfCandyBonus,Type);
    }

    private async Task DoFiveCandiesAction()
    {
        TypeOfCandyBonus typeOfBonus;
        if (PositionInSpace == PositionInSpace.Mixed)
            typeOfBonus = TypeOfCandyBonus.BombCandy;
        else
        {
            typeOfBonus = TypeOfCandyBonus.ChocolateCandy;
            Type = CandyColor.None;
        }
        await DoBonusArrayAction();
        SwitchedCandyCell.SpawnBonusCandy(typeOfBonus,Type);
    }
    
    private async Task DoBonusArrayAction()
    {
        var candiesToDestroy = matchedCandies.Where(candy => candy.cell != SwitchedCandyCell).ToList();
        foreach (var candy in candiesToDestroy)
        {
            candy.cell.Candy = null;
            candy.AssignCell(SwitchedCandyCell);
        }
        SoundManager.instance.PlaySound("CandyWrap");
        await Task.WhenAll(candiesToDestroy.Select(candy => candy.GoToYourCell(Constants.Speed)));
        await Task.WhenAll(candiesToDestroy.Select(candy => candy.DoCellGridMemberAction()));
    }
}
