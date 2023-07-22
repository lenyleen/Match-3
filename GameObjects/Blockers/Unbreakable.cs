using System;
using System.Threading.Tasks;

public class Unbreakable : Blocker, IHidingCellBlocker
{
    public Action<Cell, IHidingCellBlocker> DisableAction { get; set; }
    public override void Initialize(Cell cell)
    {
        base.Initialize(cell);
        if(cell.Candy == null) return;
        cell.Candy.Disable();
        cell.Candy.ReturnCandyToPool();
    }

    public override Task DoCellGridMemberAction()
    {
        ParticlesPlay();
        return Task.CompletedTask;
    }
    protected override void Disable()
    {
        DisableAction?.Invoke(cell,this);
        Particles.particlesAction -= Disable;
        Particles.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
