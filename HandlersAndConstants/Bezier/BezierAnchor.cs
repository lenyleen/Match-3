using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class BezierAnchor : MonoBehaviour
{
    public int CountOfCollectible
    {
        get => countOfCollectible;
        set
        {
            countOfCollectible = value;
            if(countOfCollectible <= 0)
               OnAllCollectedAction?.Invoke(this); 
        }
    }
    private int countOfCollectible;
    public event Action<BezierAnchor> OnAllCollectedAction;
    
    public List<Path> bezierPaths { get; private set; } 
    public void Initialize(int countOfCollectible,string nameOfCollectible, PathCreator[] bezierPaths)
    {
        CountOfCollectible = countOfCollectible;
        this.bezierPaths = new List<Path>();
        foreach (var pathCreator in bezierPaths)
        {
            this.bezierPaths.Add(pathCreator.path);
        }
        name = nameOfCollectible;
    }

    public Path PrepareToCollect(Transform itemTransform)
    {
        var pathToReturn = bezierPaths.FirstOrDefault(path => Math.Sign(path.Points[1].x) == Math.Sign(itemTransform.position.x));
        if (pathToReturn == null) return null;
        pathToReturn.ReassignPoints(itemTransform.transform.position, this.transform.localPosition);
        CountOfCollectible--;
        EventBus.RaiseEvent<IGoalCollectiblesObserver>(listener => listener.ItemCollected(name,CountOfCollectible));
        return pathToReturn;
    }
}
