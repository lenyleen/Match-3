using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [Header("Important Objects")] 
    [SerializeField] private CellGrid cellGridPrefab;
    [SerializeField] private InputManager inputManager;
    private CellGrid cellGrid;
    [field:SerializeField] private GUIManager GUIManger { get; set; }
    private CellGridFiller GridFiller {get; set;}
    [SerializeField] private CollectiblesManager collectiblesManager;
    [SerializeField] private ValueOfItemsDataHolder valueOfItemsData;
    public void Initialize(int lvlNumber)
    {
        var data = CandiesGridDataSaver.LoadData(lvlNumber);
        InitializeCellGrid(data);
        GridFiller = new CellGridFiller(cellGrid);
        collectiblesManager.Initialize(data.dataOfCollectiblesAsGoal);
        GUIManger.Initialize(data,valueOfItemsData, new OnLvlEndBonusCreator(cellGrid,GridFiller));
        var matchFinder = new MatchFinder(GridFiller, cellGrid);
        var candySwitcher = new CandySwitcher(matchFinder, GridFiller);
        inputManager.Initialize(candySwitcher,cellGrid);
    }
    private void InitializeCellGrid(CandiesGridDataHolder data)
    {
        cellGrid = Instantiate(cellGridPrefab, Constants.CellGridWorldPosition, Quaternion.identity);
        cellGrid.InitializeCellGridWithData(data,collectiblesManager);
    }
}
