using System;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

public class BoosterActionHandler
{
    private CellGrid cellGrid;
    private readonly Array bonusTypesOfCandies;
    private readonly Array candiesColors;
    public BoosterActionHandler(CellGrid cellGrid)
    {
        candiesColors = Enum.GetValues(typeof(CandyColor));
        bonusTypesOfCandies = Enum.GetValues(typeof(TypeOfCandyBonus));
        this.cellGrid = cellGrid;
    }
    public async Task DoBoosterAction(BoosterType boosterType, Candy candy)
    {
        switch (boosterType)
        {
            case BoosterType.Bomb:
                var bombBonusCandy = candy.cell.SpawnBonusCandy(TypeOfCandyBonus.BombCandy,candy.Color);
                await bombBonusCandy.DoCellGridMemberAction();
                break;
            case BoosterType.MagicWand:
                var typeNumber = Random.Range(0,bonusTypesOfCandies.Length - 2);
                candy.cell.SpawnBonusCandy((TypeOfCandyBonus)typeNumber,candy.Color);
                break;
            case BoosterType.Hammer:
                await candy.DoCellGridMemberAction();
                break; 
        }
    }     
}
