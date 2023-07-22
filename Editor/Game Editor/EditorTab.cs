using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorTab : IGameEditorWindow
{
    public string Name => "Lvl Editor";
    private readonly int height = Constants.Rows;
    private readonly int width = Constants.Columns;
    private const string AreaAroundCollectable = "AreaAroundCollectable";

    private int lvlNumber;
    private int numberOfMoves;
    private bool timerAsLimiter;
    private int lvlTimerInSec;
    private int lvlMaxScore;
    private int scoreGoal;
    private bool showPercentsFoldOut = false;
    
    private TypeOfObject typeOfDrawnObject;
    private CandyBrushType typeOfCandy;
    private BlockerBrushType typeOfBlocker;
    private CollectibleBrushType typeofCollectible;
    private TypeOfObject typeOfCollectibleObject;
    private CandyBrushType typeOfCandyAsCollectible;
    private BlockerBrushType typeOfBlockerAsCollectible;
    
    private Dictionary<string, Texture> tileTextures;
    private Dictionary<Texture,int> collectiblesAsGoal;
    private Texture[,] textures;
    private Texture[,] collectiblesTextures;
    private string[,] candiesTexturesNames;
    private string[,] blockersTexturesNames;
    private string[,] collectiblesTextureNames;
    private List<int> starsPercents;

    public EditorTab()
    {
        tileTextures = new Dictionary<string, Texture>();
        var editorImagesPath = new DirectoryInfo(Application.dataPath + "/FruitCandiesMatch/Editor/Resources");
        var fileInfo = editorImagesPath.GetFiles("*.png", SearchOption.TopDirectoryOnly);
        foreach (var file in fileInfo)
        {
            var filename = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            tileTextures[filename] = Resources.Load(filename) as Texture;
        }
        textures = new Texture[width, height];
        collectiblesTextures = new Texture[width, height];
        candiesTexturesNames = new string[width, height];
        blockersTexturesNames = new string[width, height];
        collectiblesTextureNames = new string[width, height];
        starsPercents = new List<int>(3){0,0,0};
        collectiblesAsGoal = new Dictionary<Texture, int>();
    }

    private readonly Dictionary<string, int> collectibleSizes = new Dictionary<string, int>()
    {
        {CollectibleBrushType.SweetStar.ToString(),-1},
        {CollectibleBrushType.WatermelonIceCream.ToString(), 2},
        {CollectibleBrushType.StrawberryMuffin.ToString(),4},
        {CollectibleBrushType.StrawberryIceCream.ToString(),6},
        {CollectibleBrushType.LayeredJuiceIceCream.ToString(),8}
    } ;
    public void DrawWindow()
    {
        GUILayout.BeginHorizontal();
        DrawLeftSide();
        GUILayout.Space(100);
        DrawRightSide();
        GUILayout.EndHorizontal();
    }
    #region LeftSide
    /// <summary>
    /// Draws left side of lvl editor
    /// </summary>
    private void DrawLeftSide()
    {
        GUILayout.BeginHorizontal(GUILayout.Width(300));
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("LEVEL CREATOR",EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "The layout settings of this level.",
            MessageType.Info);
        lvlNumber = EditorGUILayout.IntField("Number of level:",lvlNumber);
        timerAsLimiter = EditorGUILayout.Toggle("Set time as limiter", timerAsLimiter);
        if (timerAsLimiter)
            lvlTimerInSec = EditorGUILayout.IntField("Lvl timer time, sec.",lvlTimerInSec);
        else
            numberOfMoves = EditorGUILayout.IntField("Number of moves:",numberOfMoves);
        typeOfDrawnObject = (TypeOfObject)EditorGUILayout.EnumPopup("Type of drawn object", typeOfDrawnObject);
        DrawBrushSelector();
        DrawLeftSideGrid();
        GUILayout.BeginHorizontal();
        EditorGUILayout.HelpBox(
            "Shift - fill the row" +
            "\n Alt - fill the entire grid" +
            "\n Ctrl - clear the cell" +
            "\n Ctrl + Shift - clear the row",
            MessageType.Info);
        ClearTheGrid();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        LoadExistedLvl();
        Save();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    /// <summary>
    /// Draws grid of candy & blockers
    /// </summary>
    private void DrawLeftSideGrid()
    {
        GUILayout.BeginHorizontal();
        for (int column = 0; column < width; column++)
        {
            GUILayout.BeginVertical();
            for (int row = 0; row < height; row++)
            {
                DrawButton(column,row);
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }
    private void DrawButton(int column, int row)
    {
        Event curKeyboardEvent = Event.current;
        if(!GUILayout.Button(textures[column,row],GUILayout.Width(40), GUILayout.Height(40))) return;
        DrawByEvent(column,row,curKeyboardEvent);
    }
    /// <summary>
    /// Sets up button texture by keyboard event
    /// </summary>
    /// <param name="column">Selected column in grid</param>
    /// <param name="row">Selected row in grid</param>
    /// <param name="guiEvent">Keyboard event</param>
    private void DrawByEvent(int column,int row,Event guiEvent)
    {
        if (guiEvent.shift && guiEvent.control)
        {
            for (int col = 0; col < width; col++)
            {
                textures[col, row] = null;
                candiesTexturesNames[col, row] = null;
                blockersTexturesNames[col, row] = null;
            }
            return;
        }
        if (guiEvent.control)
        {
            textures[column, row] = null;
            candiesTexturesNames[column, row] = null;
            blockersTexturesNames[column, row] = null;
            return;
        }
        if (guiEvent.alt)
        {
            for (int i = 0; i <width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    DrawCandy(i,j,typeOfCandy);
                    DrawBlocker(i,j,typeOfBlocker);
                }
            }
            return;   
        }

        if (guiEvent.shift) 
        {
            for (int col = 0; col < width; col++)
            {
                DrawCandy(col, row,typeOfCandy);
                DrawBlocker(col, row,typeOfBlocker);
            }
            return;
        }
        DrawCandy(column, row,typeOfCandy);
        DrawBlocker(column, row,typeOfBlocker);
    }

    private void DrawCandy(int column,int row,CandyBrushType candyType)
    {
        if(candyType == CandyBrushType.None)
            return;
        if (blockersTexturesNames[column, row] == null)
        {
            candiesTexturesNames[column, row] = candyType.ToString();
            textures[column,row] = tileTextures[candiesTexturesNames[column, row]];
            return;
        }
        if(blockersTexturesNames[column,row] == BlockerBrushType.Unbreakable.ToString() || 
           blockersTexturesNames[column,row] == BlockerBrushType.WoodBox.ToString())
            return;
        candiesTexturesNames[column, row] = candyType.ToString();
        SetCandyButtonTexture(column,row);
    }

    private void DrawBlocker(int column,int row,BlockerBrushType blockerType)
    {
        if(blockerType == BlockerBrushType.None)
            return;
        if (candiesTexturesNames[column, row] == null || 
            blockerType is BlockerBrushType.Unbreakable or BlockerBrushType.WoodBox )
        {
            candiesTexturesNames[column,row] = null;
            blockersTexturesNames[column, row] = blockerType.ToString();
            textures[column, row] = tileTextures[blockersTexturesNames[column, row]];
            return;
        }
        blockersTexturesNames[column, row] = blockerType.ToString();
        SetCandyButtonTexture(column,row);
    }

    private void SetCandyButtonTexture(int column, int row)
    {
        var textureName = candiesTexturesNames[column,row] + blockersTexturesNames[column, row];
        if(!tileTextures.ContainsKey(textureName)) return;
        textures[column, row] = tileTextures[textureName];
    }
    private void ClearTheGrid()
    {
        if(GUILayout.Button("Clear the grid",GUILayout.Width(100)))
            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    textures[column, row] = null;
                    blockersTexturesNames[column, row] = null;
                    candiesTexturesNames[column, row] = null;
                }
            }
    }

    private void DrawBrushSelector()
    {
        if(typeOfDrawnObject == TypeOfObject.None) return;
        typeOfCandy = (CandyBrushType) EditorGUILayout.EnumPopup("Type of candy", typeOfCandy);
        typeOfBlocker = (BlockerBrushType) EditorGUILayout.EnumPopup("Type of blocker", typeOfBlocker);
    }
    /// <summary>
    /// Saves data
    /// </summary>
    private void Save()
    {
        if(!GUILayout.Button("Save",GUILayout.Width(100), GUILayout.Height(20))) return;
        if(!CheckForBlockersToCollectAndBlockersOnTheGrid())
        {
            EditorUtility.DisplayDialog(
                "Warning", "The number of blockers to collect is invalid with the number of blockers on the grid", "Ok");
            return;
        }
        if (CandiesGridDataSaver.CheckForFilesNamesEquality(lvlNumber))
            if(!EditorUtility.DisplayDialog("Warning!","A grid with the same name already exists, are you sure you want to overwrite the data","Yes","No"))
                return;
        CandiesGridDataSaver.SaveData(numberOfMoves: numberOfMoves,numberOfLevel: lvlNumber,candiesTexturesNames: candiesTexturesNames, blockersTexturesNames: blockersTexturesNames,
            collectablesTexturesNames: collectiblesTextureNames,dataOfAnotherCollectibles: collectiblesAsGoal,scoreGoal: scoreGoal, starsPercents: starsPercents,maxLvlScore: lvlMaxScore,timerTime: lvlTimerInSec,scoreAsGoal: timerAsLimiter);
    }

    private bool CheckForBlockersToCollectAndBlockersOnTheGrid()
    {
        Dictionary<string, int> blockersToCompare = new Dictionary<string, int>()
        {
            {BlockerBrushType.Bubble.ToString(), 0},
            {BlockerBrushType.Syrup.ToString(), 0}
        };
        for (int column = 0; column < width; column++)
        {
            for (int row = 0; row < height; row++)
            {
                var blockerName = blockersTexturesNames[column, row];
                if(blockerName == null) continue;
                if (blockersToCompare.ContainsKey(blockerName))
                    blockersToCompare[blockerName]++;
            }
        }
        foreach (var element in blockersToCompare)
        {
            var blockerTexture = tileTextures[element.Key];
            if (!collectiblesAsGoal.ContainsKey(blockerTexture)) continue;
            if (collectiblesAsGoal[blockerTexture] > element.Value)
                return false;
        }
        return true;
    }
    /// <summary>
    /// Loads already existed data
    /// </summary>
    private void LoadExistedLvl()
    {
        if(!GUILayout.Button("Load",GUILayout.Width(100), GUILayout.Height(20))) return;
        var dataPath = Application.dataPath;
        var path = EditorUtility.OpenFilePanel("Open level", dataPath + "/CandiesGridData",
            "asset");
        if(string.IsNullOrEmpty(path)) return;
        var fileAssetPath = path.Remove(0, dataPath.Length - 6);
        var currentLevel =
            (CandiesGridDataHolder) AssetDatabase.LoadAssetAtPath(fileAssetPath, typeof(CandiesGridDataHolder));
        UnboxData(currentLevel);
        
    }

    private void UnboxData(CandiesGridDataHolder data)
    {
        lvlNumber = data.numberOfLevel;
        scoreGoal = data.scoreGoal;
        lvlMaxScore = data.maxLvlScore;
        timerAsLimiter = data.scoreAsGoal;
        starsPercents = data.percentsOfScoreStars;
        if (timerAsLimiter)
            lvlTimerInSec = data.lvlLimitCounterNumber;
        else
            numberOfMoves = data.lvlLimitCounterNumber;
        AssignLeftGridCollections(candiesTexturesNames,data.candiesData);
        AssignLeftGridCollections(blockersTexturesNames,data.blockersData);
        var collectibleNameToEnumConverter = new Dictionary<string, CollectibleBrushType>()
        {
            {CollectibleBrushType.SweetStar.ToString(), CollectibleBrushType.SweetStar},
            {CollectibleBrushType.StrawberryMuffin.ToString(), CollectibleBrushType.StrawberryMuffin},
            {CollectibleBrushType.StrawberryIceCream.ToString(), CollectibleBrushType.StrawberryIceCream},
            {CollectibleBrushType.WatermelonIceCream.ToString(), CollectibleBrushType.WatermelonIceCream},
            {CollectibleBrushType.LayeredJuiceIceCream.ToString(), CollectibleBrushType.LayeredJuiceIceCream}
        };
        foreach (var element in data.collectiblesDataToSpawn)
        {
            if(element._element == null) continue;
            var typeOfCollectible = collectibleNameToEnumConverter[element._element.name];
            DetermineDrawability(element.Index0,element.Index1,typeOfCollectible);
        }
        foreach (var element in data.dataOfCollectiblesAsGoal)
        {
            if(collectiblesAsGoal.ContainsKey(element._element)) continue;
            collectiblesAsGoal.Add(element._element,element.Index0);
        }
    }
    private void AssignLeftGridCollections(string[,] currentCollection, List<Element<GameObject>> dataCollection)
    {
        foreach (var element in dataCollection)
        {
            if(element._element == null) continue;
            currentCollection[element.Index0, element.Index1] = element._element.GetComponent<SpriteRenderer>().sprite.name;
            SetCandyButtonTexture(element.Index0,element.Index1);
        }
    }
    #endregion

    #region RightSide
    /// <summary>
    /// Draws right side of lvl editor
    /// </summary>
    private void DrawRightSide()
    {
        GUILayout.BeginHorizontal(GUILayout.Width(300));
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("GOAL CREATOR",EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "This list defines the goals needed to be achieved by the player in order to complete this level.",
            MessageType.Info);
        lvlMaxScore = EditorGUILayout.IntField("Max lvl score", lvlMaxScore);
        showPercentsFoldOut = EditorGUILayout.BeginFoldoutHeaderGroup(showPercentsFoldOut, "Stars Percents", null, StarPercentsRightClick);
        if(showPercentsFoldOut)
        {
            for (int i = 0; i < starsPercents.Count; i++)
            {
                starsPercents[i] = EditorGUILayout.IntField($"score {i + 1} star percent, %", starsPercents[i]);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (timerAsLimiter)
        {
            scoreGoal = EditorGUILayout.IntField("Score Goal", scoreGoal);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            return;
        }
        typeofCollectible =
            (CollectibleBrushType) EditorGUILayout.EnumPopup("Type of drawn collectible", typeofCollectible);
        DrawAnotherCollectiblesSelector();
        EditorGUILayout.LabelField("Added collectibles :");
        DrawCollectiblesToCollect();
        DrawCollectibleButton();
        ClearGoal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    /// <summary>
    /// Sets up percents of lvl prize stars 
    /// </summary>
    /// <param name="rect"></param>
    private void StarPercentsRightClick(Rect rect)
    {
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("Set default percents"),false,() => 
            {starsPercents = new List<int>(){25,55,75};});
        menu.DropDown(rect);
    }
    /// <summary>
    /// Draws selector of left grid items to collect
    /// </summary>
    private void DrawAnotherCollectiblesSelector()
    {
        typeOfCollectibleObject =
            (TypeOfObject) EditorGUILayout.EnumPopup("Type of grid items as collectibles:", typeOfCollectibleObject); 
        switch (typeOfCollectibleObject)
        {
            case TypeOfObject.None:
                typeOfCandyAsCollectible = CandyBrushType.None;
                typeOfBlockerAsCollectible = BlockerBrushType.None;
                break;
            case TypeOfObject.Candy:
                typeOfCandyAsCollectible =
                    (CandyBrushType) EditorGUILayout.EnumPopup("Type of candy as collectible", 
                        typeOfCandyAsCollectible);
                CollectibleSelectorAddButton(typeOfCandyAsCollectible.ToString());
                break;
            case TypeOfObject.Blocker:
                typeOfBlockerAsCollectible =
                    (BlockerBrushType) EditorGUILayout.EnumPopup("Type of blocker as collectible", 
                        typeOfBlockerAsCollectible);
                CollectibleSelectorAddButton(typeOfBlockerAsCollectible.ToString());
                break;
        }
    }
    /// <summary>
    /// Draws ADD button to left grid items to collect
    /// </summary>
    /// <param name="textureName">Name of item texture</param>
    private void CollectibleSelectorAddButton(string textureName)
    {
        if(!tileTextures.ContainsKey(textureName)) return;
        if(!GUILayout.Button("Add",GUILayout.Width(50))) return;
        AddCollectibleToGoal(tileTextures[textureName]);
    }

    private void DrawCollectibleButton()
    {
        GUILayout.BeginHorizontal();
        for (int column = 0; column < width; column++)
        {
            GUILayout.BeginVertical();
            for (int row = 0; row < height; row++)
            {
                if (!GUILayout.Button(collectiblesTextures[column, row], GUILayout.Width(40), GUILayout.Height(40))) continue;
                DetermineDrawability(column,row,typeofCollectible);
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Checks drawability of collectible on the grid
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <param name="collectibleType"></param>
    private void DetermineDrawability(int column, int row,CollectibleBrushType collectibleType)
    {
        if(collectiblesTextures[column,row] != null && collectiblesTextures[column,row].name == AreaAroundCollectable) return;
        if (collectibleType is CollectibleBrushType.SweetStar)
        {
            DrawCollectible(column,row,collectibleType,BlockerBrushType.WoodBox,collectibleSizes[collectibleType.ToString()]);
            return;
        }
        DrawCollectible(column,row,collectibleType,BlockerBrushType.Syrup,collectibleSizes[collectibleType.ToString()]); 
    }
    /// <summary>
    /// Sets up collectible button texture, adds texture of blocker to the left grid button
    /// </summary>
    /// <param name="column">Selected column</param>
    /// <param name="row">Selected row</param>
    /// <param name="collectibleType">Type of drawn collectible</param>
    /// <param name="blockerType">Type of drawn blocker</param>
    /// <param name="size">In game size of collectible to be covered by blockers</param>
    private void DrawCollectible(int column, int row, CollectibleBrushType collectibleType,BlockerBrushType blockerType, int size)
    {
        var heightSize = size / 2; 
        var widthSize = size % 2;
        if (row > height - heightSize || column >= width - (1 + widthSize)) return;
        Dictionary<Tuple<int, int>, Texture > collectibles = new Dictionary<Tuple<int, int> ,Texture>();
        for (int i = column; i < column + 2 - widthSize; i++)
        {
            for (int j = row; j < row + heightSize; j++)
            {
                if (collectiblesTextures[i, j] != null && collectiblesTextures[i, j].name == AreaAroundCollectable)
                {
                    collectiblesTextures[column, row] = null;
                    return;
                }
                collectibles.Add(new Tuple<int, int>(i, j), tileTextures[AreaAroundCollectable]);
            }
        }
        foreach (var element in collectibles)
        {
            collectiblesTextures[element.Key.Item1, element.Key.Item2] = element.Value;
            DrawBlocker(element.Key.Item1,element.Key.Item2,blockerType);
        }

        var collectibleTexture = tileTextures[collectibleType.ToString()];
        DrawBlocker(column,row,blockerType);
        collectiblesTextures[column, row] = collectibleTexture;
        collectiblesTextureNames[column, row] = typeofCollectible.ToString();
        AddCollectibleToGoal(collectibleTexture);
    }
    /// <summary>
    /// Adds collectible to goal data
    /// </summary>
    /// <param name="collectibleTexture">Texture of collectible</param>
    private void AddCollectibleToGoal(Texture collectibleTexture)
    {
        if (collectiblesAsGoal.Keys.Contains(collectibleTexture))
        {
            collectiblesAsGoal[collectibleTexture]++;
            return;
        }
        if(collectiblesAsGoal.Count >= 3) return;
        collectiblesAsGoal.Add(collectibleTexture,1);
    }

    private void DrawCollectiblesToCollect()
    {
        GUILayout.BeginHorizontal();
        foreach (var element in collectiblesAsGoal)
        {
            EditorGUILayout.LabelField(new GUIContent(element.Key),GUILayout.Width(20));
            EditorGUILayout.LabelField(element.Value.ToString(),GUILayout.Width(10));
        }
        GUILayout.EndHorizontal();
    }
    /// <summary>
    /// Clears right grid, sets up goal of collect by default
    /// </summary>
    private void ClearGoal()
    {
        if(!GUILayout.Button("Clear Grid")) return;
        collectiblesTextures = new Texture[width, height];
        collectiblesAsGoal = new Dictionary<Texture, int>();
    }
    #endregion
    
    
    
    enum TypeOfObject
    {
        None,
        Candy,
        Blocker
    }
    enum CandyBrushType
    {
        None, 
        YellowItem,
        BlueItem,
        RedItem,
        OrangeItem,
        GreenItem,
        PurpleItem,
        RandomItem
    }
    enum BlockerBrushType
    {
        None,
        Syrup,
        Bubble,
        Unbreakable,
        WoodBox
    }
    enum CollectibleBrushType
    {
        LayeredJuiceIceCream,
        StrawberryIceCream,
        StrawberryMuffin,
        WatermelonIceCream,
        SweetStar 
    }
}
