using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public sealed class NormalCandy : Candy
{
    
    public override async Task DoCellGridMemberAction()
    {
        await base.DoCellGridMemberAction();
        HitNearBeforeDie();
    }
}
