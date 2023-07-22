using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))][RequireComponent(typeof(PooledObject))]
public class Candy : MonoBehaviour, IEquatable<Candy>, ICellGridMember, ICanBeCollected
{
    public string collectibleId => Sprite.sprite.name;
    [field: SerializeField]public Cell cell { get; private set; }
    private Particles Particles;
    [field: SerializeField]public CandyColor Color { get; private set; }
    [field: SerializeField]public TypeOfCandyBonus TypeOfBonus { get; private set; }
    [SerializeField] protected LayerMask blockersLayer;
    [SerializeField] protected LayerMask ignoreRaycastLayer;
    [SerializeField] protected LayerMask defaultLayer;
    protected BoxCollider2D boxCollider => this.GetComponent<BoxCollider2D>();
    [field: SerializeField]public bool Blocked { get; private set; }
    private Animator Animator => this.GetComponent<Animator>();
    private SpriteRenderer Sprite => GetComponent<SpriteRenderer>();
    [field: SerializeField]public CollectiblesManager CollectiblesManager { get; set; }
    private PooledObject pooledObject;
    
    protected virtual void Awake()
    {
        pooledObject = this.GetComponent<PooledObject>();
        name = Sprite.name;
    }
    protected virtual void OnEnable()
    {
        
        Animator.enabled = true;
        boxCollider.enabled = true;
    }
    public void InitializeAsCollectible(CollectiblesManager collectiblesManager)
    {
        CollectiblesManager = collectiblesManager;
    }
    public void ParticlesPlay()
    {
        EventBus.RaiseEvent<IScoreItemObserver>(listener => listener.ItemCollected(collectibleId));
        GetParticles();
        PlayAnimation(CandyAnimationTriggers.Kill);
        Particles.Play();
    }
    public void SetBlocked(Bubble bubble)
    {
        Blocked = true;
        bubble.OnDestroyAction += (() => { this.Blocked = false;});
    }
    public void AssignCell(Cell cell)
    {
        var thisTransform = this.transform;
        this.cell = cell;
        thisTransform.SetParent(cell.transform);
    }
    public virtual async Task DoCellGridMemberAction()
    {
        boxCollider.enabled = false;
        if (CollectiblesManager == null)
        {
            ReturnCandyToPool();
            return;
        }
        var bezierAnchor = CollectiblesManager.CheckItemForCollectibility(this.gameObject);
        if (bezierAnchor != null)
        {
            var b = bezierAnchor.PrepareToCollect(this.transform);
            await Collect(b); 
        }
        else
            ParticlesPlay();
        ReturnCandyToPool();
    }

    public void ReturnCandyToPool()
    {
        if (cell != null && cell.Candy == this)
            cell.Candy = null;
        cell = null;
        pooledObject.ReturnToPool();
    }
    public void GetParticles()
    {
        if(Particles != null)
            return;
        Particles = ObjectsPool.Pool.GetParticle(this);
        Particles.transform.position = this.transform.position;
        Particles.particlesAction += Disable;
    }
    protected void HitNearBeforeDie()
    {
        var hits = new List<RaycastHit2D>();
        var castFilter = new ContactFilter2D();
        castFilter.SetLayerMask(ignoreRaycastLayer);
        Physics2D.CircleCast(this.transform.position, 0.1f, Vector2.zero, castFilter, hits);
        if (hits.Count > 0)
        {
            foreach (var hit2D in hits)
            {
                hit2D.transform.GetComponent<ICellGridMember>().DoCellGridMemberAction();
            }
        }
        hits = new List<RaycastHit2D>();
        castFilter = new ContactFilter2D();
        castFilter.SetLayerMask(blockersLayer);
        Physics2D.CircleCast(this.transform.position, Constants.StandardCircleCastRadius, Vector2.zero, castFilter, hits);
        if(hits.Count <= 0) return;
        foreach (var hit2D in hits)
        {
            hit2D.transform.GetComponent<ICellGridMember>().DoCellGridMemberAction();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position,Constants.StandardCircleCastRadius);
    }

    public void PlayAnimation(CandyAnimationTriggers animationTrigger)
    {
        Animator.SetTrigger(animationTrigger.ToString());
    }

    public void ReassignCell(Cell otherHitCandyCell, Cell thisHitCandyCell, Candy otherHitCandy)
    {
        AssignCell(otherHitCandyCell);
        otherHitCandy.AssignCell(thisHitCandyCell);
        cell.Assign(this);
        otherHitCandy.cell.Assign(otherHitCandy);
    }
    public void Disable()
    {
        if (Particles != null)
            Particles.particlesAction -= Disable;
        Particles = null;
        Animator.enabled = false;
    }
    public virtual bool Equals(Candy other)
    {
        if (other != null) return this.Color == other.Color;
        return false;
    }
    public async Task Collect(Path path)
    {
        boxCollider.enabled = false;
        float t = 0;
        while (t < 1)
        {
            this.transform.position = Bezier.GetPoint(this.transform.position,path.Points[1],path.Points[2],path.Points[3],t);
            t += 3.5f * Time.deltaTime;
            await Task.Delay(1);
        }
        Disable();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        this.Sprite.enabled = true;
    }
    public async Task GoToYourCell(float speed)
    {
        var t = 0f;
        while (t < 1)
        {
            t +=  (speed / 5) * 0.02f;
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero, t);
            await Task.Delay(1);
        }
        PlayAnimation(CandyAnimationTriggers.Falling);
    }
}

public enum CandyAnimationTriggers
{
    Pressed,
    SuggestedMatch,
    Kill,
    Falling,
    Reset,
    Unpressed
}
