using System;
using System.Threading.Tasks;
using UnityEngine;

public class WoodBox : Blocker, IHidingCellBlocker
{
    public Action<Cell,IHidingCellBlocker> DisableAction { get; set; }
    public override void Initialize(Cell cell)
    {
        base.Initialize(cell);
        if(cell.Candy == null) return;
        cell.Candy.Disable();
        cell.Candy.ReturnCandyToPool();
    }
    public override Task DoCellGridMemberAction()
    {
        DisableAction?.Invoke(cell,this);
        ParticlesPlay();
        return Task.CompletedTask;
    }
    protected override void Disable()
    {
        Particles.particlesAction -= Disable; 
        Particles.gameObject.SetActive(false); 
        Destroy(this.gameObject);
    }
}
