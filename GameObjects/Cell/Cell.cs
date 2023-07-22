using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class Cell : MonoBehaviour, IComparable<Cell>
{
    [field: SerializeField]public Candy Candy { get; set; }
    [field: SerializeField]public int Row { get; private set; }
    [field: SerializeField]public int Column { get; private set; }
    [SerializeField]private Particles particles;
    private CollectiblesManager collectiblesManager;
    public bool Blocked { get; private set; }
    private int ID { get; set;}
    public int matchId;

    public void Initialize(int column, int row, int id, CollectiblesManager collectiblesManager)
    {
        Row = row;
        Column = column;
        this.ID = id;
        matchId = id;
        this.collectiblesManager = collectiblesManager;
    }
    public Candy SpawnCandy(Candy candy)
    {
        Assign(candy);
        candy.AssignCell(this);
        candy.transform.localPosition = Vector3.zero;
        candy.InitializeAsCollectible(collectiblesManager);
        return candy;
    }

    public Candy SpawnBonusCandy(TypeOfCandyBonus bonusType,CandyColor color)
    {
        var candy = ObjectsPool.Pool.GetBonusCandy(color, bonusType);
        if (candy == null) return null;
        candy.InitializeAsCollectible(collectiblesManager);
        this.Candy.Disable();
        this.Candy.ReturnCandyToPool();
        ParticlesPlay();
        Assign(candy);
        candy.AssignCell(this);
        candy.transform.localPosition = Vector3.zero;
        return candy; 
    }

    public void SetBlocked(ICellBlockerBlocker blocker)
    {
        Blocked = true;
        blocker.OnDestroyAction += () => { Blocked = false;};
    }

    public T SpawnBlocker<T>(T blockerPrefab) where T : Blocker
    {
        if(blockerPrefab == null) return null;
        var thisTransform = this.transform;
        var blocker = Instantiate(blockerPrefab, thisTransform.position, quaternion.identity, thisTransform);
        blocker.Initialize(this);
        blocker.InitializeAsCollectible(collectiblesManager);
        return blocker;
    }
    public void Assign(Candy candy)
    { 
        matchId = ID;
        this.Candy = candy;
    }
    public bool Matched() => ID != matchId; 
    public void SetIdByDefault()
    {
        matchId = ID;
    }
    public bool IsEquals(Cell other)
    {
        return this.Row == other.Row && this.Column == other.Column;
    }

    public bool IsNearestCell(Cell other)
    {
        var deltaRow = Mathf.Abs(this.Row - other.Row);
        var deltaColumn = Mathf.Abs(this.Column - other.Column);
        return deltaColumn + deltaRow == 1;
    }
    private void ParticlesPlay()
    {
        particles.gameObject.SetActive(true);
    }
    public int CompareTo(Cell other)
    {
        return Column.CompareTo(other.Column);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position,0.5f);
    }
}
