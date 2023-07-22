using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public static class CandiesGridDataSaver
{
#if UNITY_EDITOR
    private const string AreaAroundCollectable = "AreaAroundCollectable";
    private static void CreateMyAsset(CandiesGridDataHolder asset,string name)
    {
        AssetDatabase.CreateAsset(asset, $"Assets/Resources/CandiesGridData/{name}.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    private static string assetPrefix = "Level_";

    public static void SaveData(int numberOfMoves,int numberOfLevel, string[,] candiesTexturesNames,
        string[,] blockersTexturesNames, string[,] collectablesTexturesNames, 
        Dictionary<Texture,int> dataOfAnotherCollectibles, int scoreGoal, List<int> starsPercents, int maxLvlScore,int timerTime,bool scoreAsGoal)
    {
        string fullName = assetPrefix + numberOfLevel.ToString();
        var data = ScriptableObject.CreateInstance<CandiesGridDataHolder>();
        var prefabsPath = new DirectoryInfo(Application.dataPath + "/Resources/Prefabs");
        var fileInfo = prefabsPath.GetFiles("*.prefab", SearchOption.AllDirectories);
        var candies = AttachElements(candiesTexturesNames,fileInfo);
        var blockers = AttachElements(blockersTexturesNames,fileInfo);
        var coll = AttachElements(collectablesTexturesNames,fileInfo);
        var anotherColl = UnboxAnotherCollectiblesData(dataOfAnotherCollectibles);
        var lvlLimitCounterNumber = scoreAsGoal ? timerTime : numberOfMoves;
        data.Initialize(lvlLimitCounterNumber,numberOfLevel,candies, blockers,coll,anotherColl, scoreGoal, starsPercents,maxLvlScore,scoreAsGoal);
        CreateMyAsset(data,fullName);
    }

    

    private static List<Element<Texture>> UnboxAnotherCollectiblesData(Dictionary<Texture,int> anotherCollectiblesData)
    {
        var dataOfAnotherCollectibles = new List<Element<Texture>>();
        if (anotherCollectiblesData.Count <= 0) return null;
        foreach (var item in anotherCollectiblesData)
        {
            dataOfAnotherCollectibles.Add(new Element<Texture>(item.Value,0,item.Key));
        }

        return dataOfAnotherCollectibles;
    }
    private static List<Element<GameObject>> AttachElements(string[,] texturesNames, FileInfo[] fileInfos)
    {
        string randomCandyPrefabPath = @"\\\w{1,6}Candy.prefab";
        var randomCandyFullPathOfPrefab = fileInfos.Where(c => Regex.IsMatch(c.ToString(), randomCandyPrefabPath)).ToList();
        GameObject[] gameObjectsLoadedPrefabs = new GameObject[randomCandyFullPathOfPrefab.Count()];
        for (int k = 0; k < randomCandyFullPathOfPrefab.Count(); k++)
        {
            var randomCandyAssetsFolderPathOfPrefab = randomCandyFullPathOfPrefab[k]?.FullName.Remove(0, Application.dataPath.Length - 6);
            gameObjectsLoadedPrefabs[k] = (GameObject) AssetDatabase.LoadAssetAtPath(randomCandyAssetsFolderPathOfPrefab, typeof(GameObject));
        }
        List<Element<GameObject>> gameObjectsToSave = new List<Element<GameObject>>();
        for (int i = 0; i < texturesNames.GetLength(0); i++)
        {
            for (int j = 0; j < texturesNames.GetLength(1); j++)
            {
                if(texturesNames[i,j] != null)
                {
                    switch (texturesNames[i,j])
                    {
                        case "Hole":
                            gameObjectsToSave.Add(new Element<GameObject>(i,j,null));
                            break;
                        case "RandomItem":
                            
                            gameObjectsToSave.Add(new Element<GameObject>(i,j,
                                CompareCandiesForRandomization(gameObjectsLoadedPrefabs,texturesNames,i,j)));
                            break;
                        default:
                            var prefabPath = @"\" + $"{texturesNames[i,j]}.prefab";
                            var fullPathOfPrefab = fileInfos.FirstOrDefault(c => c.ToString().Contains(prefabPath));
                            var assetsFolderPathOfPrefab = fullPathOfPrefab?.FullName.Remove(0, Application.dataPath.Length - 6);
                            var loadedGameObject = (GameObject) AssetDatabase.LoadAssetAtPath(assetsFolderPathOfPrefab, typeof(GameObject));
                            gameObjectsToSave.Add(new Element<GameObject>(i,j,loadedGameObject));
                            break;
                    }
                }
            }   
        }
        return gameObjectsToSave;
    }

    private static GameObject CompareCandiesForRandomization(GameObject[] gameObjectsPrefabs ,string[,] texturesNames,int row, int column)
    {
        var newCandy = GetRandomCandy(gameObjectsPrefabs);
        while (row >= 1 && texturesNames[(row - 1), column] != null && IsCandiesEquals(texturesNames[row-1,column], newCandy.name)
               || column >= 1 && texturesNames[row, (column - 1)] != null && IsCandiesEquals(texturesNames[row,column -1], newCandy.name))
        { 
            newCandy = GetRandomCandy(gameObjectsPrefabs);
        }

        texturesNames[row, column] = newCandy.name;
        return newCandy;
    }

    private static bool IsCandiesEquals(string textureName, string newCandy)
    {
        return textureName == newCandy;
    }
    private static GameObject GetRandomCandy(GameObject[] gameObjectsPrefabs)
    {
        return gameObjectsPrefabs[Random.Range(0,gameObjectsPrefabs.Length)];
    }
    public static bool CheckForFilesNamesEquality(int numberOfLevel)
    {
        var assetsPath = new DirectoryInfo("Assets/Resources/CandiesGridData");
        var fileInfo = assetsPath.GetFiles("*.asset", SearchOption.TopDirectoryOnly);
        foreach (var file in fileInfo)
        {
            var filename = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            filename = filename.Remove(0,assetPrefix.Length);
            if (filename == numberOfLevel.ToString())
                return true;
        }
        return false;
    }
#endif
    public static CandiesGridDataHolder LoadData(int numberOfLevel)
    {
        var assetsPath = Resources.Load<CandiesGridDataHolder>($"CandiesGridData/Level_{numberOfLevel.ToString()}");
        return assetsPath;
    }
}

