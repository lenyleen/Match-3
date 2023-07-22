using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameObjects.Candies;
using Unity.VisualScripting;

public sealed class SwitchedBonusCandiesHandler
{
    private CellGridFiller cellGridFiller;
    public SwitchedBonusCandiesHandler(CellGridFiller cellGridFiller)
    {
        this.cellGridFiller = cellGridFiller;
    }
    public async Task<bool> DefineBonusMatch(Candy hitCollider, Candy hitCandy)
    {
        if(hitCandy.TypeOfBonus != TypeOfCandyBonus.None)
            return await hitCandy.GetComponent<BonusCandy>().BonusAction(hitCollider);
        if(hitCollider.TypeOfBonus != TypeOfCandyBonus.None)
            return await hitCollider.GetComponent<BonusCandy>().BonusAction(hitCandy);
        return false;
    }
}
