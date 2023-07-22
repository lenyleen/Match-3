using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class CellGrid : MonoBehaviour
{
    private readonly Cell[,] cellGrid = new Cell[Constants.Columns, Constants.Rows];
    [SerializeField] private Cell cellPrefab;
    public float DistanceBetweenCells { get;private set; }
    private Vector3 Position => this.transform.position;
    private CollectiblesManager collectiblesManager;
    public float LastCandyYPosition { get; private set; }
    public Cell this[int column, int row]
    {
        get
        {
            try
            {
                return cellGrid[column, row];
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        set => cellGrid[column, row] = value;
    }
    public int GetLength(int dimension) => cellGrid.GetLength(dimension);
    public void InitializeCellGridWithData(CandiesGridDataHolder data,CollectiblesManager collectiblesManager)
    {
        this.collectiblesManager = collectiblesManager;
        for (int column = 0; column < GetLength(0); column++)
        {
            for (int row = 0; row < GetLength(1); row++)
            {
                var newCell = Instantiate(cellPrefab,
                    new Vector2(Position.x + column * Constants.CellSize.x,
                        Position.y - row * Constants.CellSize.y), Quaternion.identity,
                    this.transform);
                var id = (row * GetLength(0)) + column;
                cellGrid[column, row] = newCell;
                newCell.Initialize(column,row,id, collectiblesManager);
            }
        }
        LastCandyYPosition = cellGrid[GetLength(0) - 1, GetLength(1) - 1].transform.position.y;
        DistanceBetweenCells =  Mathf.Abs(Mathf.Abs(cellGrid[0, 0].transform.position.y) -
                                            Mathf.Abs(cellGrid[0, 1].transform.position.y));
        InitializeCandies(data);
    }
    private void InitializeCandies(CandiesGridDataHolder data)
    {
        foreach (var candyElement in data.candiesData)
        {
            if(cellGrid[candyElement.Index0,candyElement.Index1] == null)
                continue;
            var candy = candyElement._element.GetComponent<Candy>();
            if(candy == null) continue;
            cellGrid[candyElement.Index0,candyElement.Index1].SpawnCandy(ObjectsPool.Pool.GetCandy(candy));
        }

        SpawnBlockersOrCollectibles(data.blockersData,data.collectiblesDataToSpawn);
    }
    private void SpawnBlockersOrCollectibles(List<Element<GameObject>> blockersPrefabs, List<Element<GameObject>> collectiblesPrefabs)
    {
        for (int i = 0; i < collectiblesPrefabs.Count; i++)
        {
            var collectible = collectiblesPrefabs[i];
            var newCollectible = Instantiate(collectible._element,
                cellGrid[collectible.Index0, collectible.Index1].transform.position,
                collectible._element.transform.rotation).GetComponent<Collectible>();
            newCollectible.InitializeAsCollectible(collectiblesManager);
            
        }
        for (int i = 0; i < blockersPrefabs.Count; i++)
        {
            var blockerIndexes = new Tuple<int, int>(blockersPrefabs[i].Index0, blockersPrefabs[i].Index1);
            var cell = cellGrid[blockerIndexes.Item1, blockerIndexes.Item2];
            var blocker = cell.SpawnBlocker(blockersPrefabs[i]._element.GetComponent<Blocker>());
            if (blocker is not IHidingCellBlocker hidingCellBlocker) continue;
            Action<Cell, IHidingCellBlocker> handler = null;
            handler = (blockerCell, blockerObj) =>
            {
                { cellGrid[blockerCell.Column, blockerCell.Row] = blockerCell;
                    blockerObj.DisableAction -= handler; }
            };
            hidingCellBlocker.DisableAction += handler;
            cellGrid[blockerIndexes.Item1, blockerIndexes.Item2] = null;
        }
    }

    public Candy SpawnRandomCandy(int column, int row)
    {
        var colors = Enum.GetValues(typeof(CandyColor));
        var randomColor = Random.Range(0,colors.Length - 2); 
        while ((row >= 1 && cellGrid[column,row - 1] != null && cellGrid[column,row - 1].Candy != null && (CandyColor)randomColor == cellGrid[column,row - 1].Candy.Color) 
               || (column >= 1 && cellGrid[column - 1,row] != null && cellGrid[column - 1,row].Candy != null  && (CandyColor)randomColor == cellGrid[column - 1,row].Candy.Color))
        {
            randomColor = Random.Range(0,colors.Length - 2); 
        }
        var candyColor = (CandyColor) randomColor;
        return cellGrid[column,row].SpawnCandy(ObjectsPool.Pool.GetCandyByColor(candyColor));
    }

    public Candy SpawnRandomBonusCandyWithRandomCell()
    {
        var colors = Enum.GetValues(typeof(CandyColor));
        var randomColor = Random.Range(0,colors.Length - 2);
        var randomBonuses = Enum.GetValues(typeof(TypeOfCandyBonus));
        var randomBonus = Random.Range(1, randomBonuses.Length - 1);
        var randomColumn = Random.Range(0, Constants.Columns);
        var randomRow = Random.Range(0, Constants.Rows);
        var randomCell = cellGrid[randomColumn, randomRow];
        while (randomCell == null)
        {
            randomColumn = Random.Range(0, Constants.Columns);
            randomRow = Random.Range(0, Constants.Rows);
            randomCell = cellGrid[randomColumn, randomRow];
        }
        return randomCell.SpawnBonusCandy((TypeOfCandyBonus)randomBonus,(CandyColor)randomColor);
    }
}
