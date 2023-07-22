using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Collectible : MonoBehaviour, ICanBeCollected, IMoveEndedObserver
{
    public string collectibleId => GetComponent<SpriteRenderer>().sprite.name;
    [field: SerializeField] public int Size { get; private set; }
    private BoxCollider2D _collider2D => GetComponent<BoxCollider2D>();
    private Vector3 delta { get; set; }
    private Vector3 centerOfObj => this.transform.position - delta;
    [SerializeField] protected LayerMask blockersLayer;
    public CollectiblesManager CollectiblesManager { get; set; }
    public async Task Collect(Path path)
    {
        EventBus.RaiseEvent<IScoreItemObserver>(listener => listener.ItemCollected(collectibleId));
        float t = 0;
        while (t < 1)
        {
            this.GetComponent<SpriteRenderer>().sortingOrder = 10;
            this.transform.position = Bezier.GetPoint(this.transform.position,path.Points[1],path.Points[2],path.Points[3],t);
            t += 2.5f * Time.deltaTime;
            this.transform.localScale /= 1 + t;
            await Task.Delay(1);
        }
        SoundManager.instance.PlaySound("Collectable");
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        EventBus.Subscribe(this);
        delta = this.transform.position - _collider2D.bounds.center;
        _collider2D.enabled = false;
    } 
    public void InitializeAsCollectible(CollectiblesManager collectiblesManager)
    {
        CollectiblesManager = collectiblesManager;
    }
    public async void MoveEnded()
    {
        var hits = new List<RaycastHit2D>();
        var colliderSize = _collider2D.size;
        var thisLocalScale = this.transform.localScale;
        ContactFilter2D newFilter = new ContactFilter2D();
        newFilter.SetLayerMask(blockersLayer);
        Physics2D.BoxCast(centerOfObj,
            Size > 2
                ? new Vector2((_collider2D.size.x - 0.2f) * this.transform.localScale.x,
                    (colliderSize.y - 0.2f) * thisLocalScale.y)
                : new Vector2((_collider2D.size.y - 0.2f) * this.transform.localScale.y,
                    (colliderSize.x - 0.2f) * thisLocalScale.x), 0f, Vector2.zero, newFilter, hits);
        if(hits.Count > 0) return;
        var bezierAnchor = CollectiblesManager.CheckItemForCollectibility(this.gameObject);
        if (bezierAnchor != null)
            await Collect(bezierAnchor.PrepareToCollect(this.transform));
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }
}
