using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GameObjects.Candies;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectsPool : MonoBehaviour
{
    public static ObjectsPool Pool;
    [SerializeField]private List<ObjectsPoolHolder> normalPoolsPrefabs;
    [SerializeField]private List<ObjectsPoolHolder> verticalPoolsPrefabs;
    [SerializeField]private List<ObjectsPoolHolder> horizontalPoolsPrefabs;
    [SerializeField]private List<ObjectsPoolHolder> bombsPoolsPrefabs;
    [SerializeField] private List<ObjectsPoolHolder> chocolateCandyPoolPrefab;
    [SerializeField]private List<ObjectsPoolHolder> normalParticlesPoolsPrefabs;
    [SerializeField]private List<ObjectsPoolHolder> verticalParticlesPoolsPrefabs;
    [SerializeField]private List<ObjectsPoolHolder> horizontalParticlesPoolsPrefabs;
    [SerializeField]private List<ObjectsPoolHolder> bombsParticlesPoolsPrefabs;
    [SerializeField]private List<ObjectsPoolHolder> chocolateCandyParticlesPoolPrefab;
    private Dictionary<TypeOfCandyBonus, List<ObjectsPoolHolder>> _particlesDictionary;
    private Dictionary<TypeOfCandyBonus, List<ObjectsPoolHolder>> _candiesDictionary;
    private void Awake()
    {
        if (Pool == null) Pool = this;
        InitializePoll();
    }
    private void InitializePoll()
    {
        var normalCandies = InstantiateObjectsPools(normalPoolsPrefabs);
        var verticalCandies = InstantiateObjectsPools(verticalPoolsPrefabs);
        var horizontalCandies = InstantiateObjectsPools(horizontalPoolsPrefabs);
        var bombBonusCandies = InstantiateObjectsPools(bombsPoolsPrefabs);
        var chocolateCandiesPool = InstantiateObjectsPools(chocolateCandyPoolPrefab);
        _candiesDictionary = new Dictionary<TypeOfCandyBonus, List<ObjectsPoolHolder>>()
        {
            {TypeOfCandyBonus.None, normalCandies},
            {TypeOfCandyBonus.StripedHorizontalCandy, horizontalCandies},
            {TypeOfCandyBonus.StripedVerticalCandy, verticalCandies},
            {TypeOfCandyBonus.BombCandy, bombBonusCandies},
            {TypeOfCandyBonus.ChocolateCandy, chocolateCandiesPool}
        };
        var normalParticles = InstantiateObjectsPools(normalParticlesPoolsPrefabs);
        var horizontalParticles = InstantiateObjectsPools(horizontalParticlesPoolsPrefabs);
        var verticalParticles = InstantiateObjectsPools(verticalParticlesPoolsPrefabs);
        var bombBonusParticles = InstantiateObjectsPools(bombsParticlesPoolsPrefabs);
        var chocolateParticles = InstantiateObjectsPools(chocolateCandyParticlesPoolPrefab);
        
        
        _particlesDictionary = new Dictionary<TypeOfCandyBonus, List<ObjectsPoolHolder>>()
        {
            {TypeOfCandyBonus.None, normalParticles},
            {TypeOfCandyBonus.StripedHorizontalCandy, horizontalParticles},
            {TypeOfCandyBonus.StripedVerticalCandy, verticalParticles},
            {TypeOfCandyBonus.BombCandy, bombBonusParticles},
            {TypeOfCandyBonus.ChocolateCandy, chocolateParticles}
        };
    }

    private List<ObjectsPoolHolder> InstantiateObjectsPools(List<ObjectsPoolHolder> poolPrefab)
    {
        return poolPrefab.Select(pool => Instantiate(pool, new Vector3(-12f, -12f, 0), Quaternion.identity)).ToList();
    }
    public Candy GetBonusCandy(CandyColor color,TypeOfCandyBonus bonusCandyType)
    {
        return _candiesDictionary[bonusCandyType].FirstOrDefault(pool => pool.type == color)?.GetObject<Candy>();
    }
    
    public Candy GetCandy(Candy candy)
    {
        return _candiesDictionary[candy.TypeOfBonus].FirstOrDefault(pool => pool.type == candy.Color)?.GetObject<Candy>();
    }
    public Particles GetParticle(Candy candy)
    {
        if(candy.TypeOfBonus == TypeOfCandyBonus.None)
            return _particlesDictionary[candy.TypeOfBonus].FirstOrDefault(pool => pool.type == candy.Color)
                ?.GetObject<Particles>();
        return _particlesDictionary[candy.TypeOfBonus].FirstOrDefault()
            ?.GetObject<Particles>();
    }
    public Candy GetCandyByColor(CandyColor color)
    {
        return _candiesDictionary[TypeOfCandyBonus.None].FirstOrDefault(pool => pool.type == color)?.GetObject<Candy>();
    }
}
