using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameObjects.Candies;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class VerticalAndHorizontalBonus : BonusCandy
{
    [field: SerializeField] private PositionInSpace PositionInSpace { get; set; }

    private readonly Vector2[] _horizontalRayDirections = {Vector2.left, Vector2.right};
    private readonly Vector2[] _verticalRayDirections = {Vector2.down, Vector2.up};
    private Dictionary<PositionInSpace, Vector2[]> directionSeparator;
    private Dictionary<PositionInSpace, Vector3[]> directionForSwitchBonus;
    protected override void Awake()
    {
        base.Awake();
        directionSeparator = new Dictionary<PositionInSpace, Vector2[]>()
        {
            {PositionInSpace.Horizontal, _horizontalRayDirections},
            {PositionInSpace.Vertical, _verticalRayDirections}
        };
        directionForSwitchBonus = new Dictionary<PositionInSpace, Vector3[]>()
        {
            {PositionInSpace.Vertical,new Vector3[]{new Vector3(Constants.DistanceBetweenCells,0f,0),
                new Vector3(-Constants.DistanceBetweenCells,0f,0f)}},
            {PositionInSpace.Horizontal, new Vector3[]{new Vector3(0f,Constants.DistanceBetweenCells,0f), 
                new Vector3(0f,- Constants.DistanceBetweenCells,0f)}}
        };
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
    public override async Task DoCellGridMemberAction()
    {
        await base.DoCellGridMemberAction();
        foreach (var direction in directionSeparator[PositionInSpace])
        {
            CastRayToDestroyCandies(direction);
        }
        HitNearBeforeDie();
        SoundManager.instance.PlaySound("LineVerticalHorizontal");
        ParticlesPlay();
    }
    private void CastRayToDestroyCandies(Vector2 direction, Vector3 position = default)
    {
        var hits = new List<RaycastHit2D>();
        var castFilter = new ContactFilter2D();
        castFilter.SetLayerMask(defaultLayer);
        Physics2D.Raycast(this.transform.position, direction, castFilter, hits, 50f);
        foreach (var collider in hits)
        {
            collider.transform.GetComponent<ICellGridMember>().DoCellGridMemberAction();
        }
    }
    public override async Task SuperBonus()
    {
        await base.SuperBonus();
        var lis = new List<Candy>(){this};
        var newPosition = directionForSwitchBonus[PositionInSpace];
        for (var i = 0; i < 2; i++)
        {
            var newCandy = ObjectsPool.Pool.GetCandy(this);
            newCandy.gameObject.SetActive(true);
            newCandy.transform.position = this.transform.position + newPosition[i];
            newCandy.GetComponent<SpriteRenderer>().enabled = false;
            lis.Add(newCandy);
        }
        await Task.WhenAll(lis.Select(candy => candy.DoCellGridMemberAction()));
    }
}
