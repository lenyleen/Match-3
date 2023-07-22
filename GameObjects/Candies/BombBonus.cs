using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameObjects.Candies;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public sealed class BombBonus : BonusCandy
{
    public override async Task DoCellGridMemberAction()
    {
        await base.DoCellGridMemberAction();
        HItNear(1.5f);
        HitNearBeforeDie();
    }

    private void HItNear(float radiusOfHit)
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
        var hits = new List<RaycastHit2D>();
        var castFilter = new ContactFilter2D();
        castFilter.SetLayerMask(defaultLayer);
        Physics2D.CircleCast(this.transform.position, radiusOfHit, Vector2.zero, castFilter, hits);
        SoundManager.instance.PlaySound("CandyWrap");
        ParticlesPlay();
        List<Candy> hitCandies = new List<Candy>();
        foreach (var collider in hits)
        {
            collider.transform.GetComponent<ICellGridMember>().DoCellGridMemberAction();
            if(collider.transform.GetComponent<Candy>() != null)
                hitCandies.Add(collider.transform.GetComponent<Candy>());
        }
        this.GetComponent<BoxCollider2D>().enabled = true;
    }

    public override Task SuperBonus()
    {
        base.SuperBonus();
        HItNear(3f);
        return Task.CompletedTask;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position,2f);
    }
    public override async Task<bool> BonusAction(Candy candy)
    {
        if (candy.TypeOfBonus == TypeOfCandyBonus.None)
            return false;
        if(candy.TypeOfBonus != TypeOfCandyBonus.ChocolateCandy)
        {
            await Task.WhenAll(candy.GetComponent<BonusCandy>().SuperBonus(), SuperBonus());
            return true;
        }
        await candy.GetComponent<BonusCandy>().BonusAction(this);
        return true;
    }
}
