using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static bool[,] level1Arr = new bool[12, 12]; // static array of level 1
    private static List<bool[,]> levelMasterArray = new List<bool[,]>();
    
    private static List<Tuple<int, int>> enemySpawnPoints = new List<Tuple<int,int>>(); // list of enemy spawn points (x, y)
    private static List<Tuple<int, int>> goals = new List<Tuple<int,int>>();  // list of the 

    // int array to store the size of each level
    private static int[] row = {12};
    private static int[] col = {12};
    private int currentLevel;
  
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = GameManager.getCurrentLevel(); // gets the current level from zach's function
        //Debug.Log("Current level: " + currentLevel);
        levelMasterArray.Add(level1Arr);
        enemySpawnPoints.Add(new Tuple<int, int>(0, 5));
        goals.Add(new Tuple<int, int>(11, 5));
        GameManager.setLevelArray(levelMasterArray[currentLevel]);
        Initiate_Level(currentLevel);
        
        /*
        switch(currentLevel)
        {
            case 0:
                Initialize_Level_1();
                break;
            default:
                Debug.Log("Current level invalide while initializing array");
                break;
        } 
        */
    }

    void Initiate_Level(int currentLevel)
    {
        switch (currentLevel)
        {
            case 0:
                Initialize_Level_1();
                break;
            default:
                Debug.Log("Current level invalide while initializing array");
                break;
        }
    }
    void Initialize_Level_1()
    {
        /// Initializes the level 1 bool array of traversable tiles.
        ///     - true  = is traversable
        ///     - false = not traversable
        for (int i = 0; i < row[0]; i++)
        {
            for (int j = 0; j < col[0]; j++)
            {
                // set positions certain to non-traversable tiles
                if ((i == 1 && j == 2) || (i == 1 && j == 9) || 
                    (i == 4 && j == 2) || (i == 4 && j == 9) || 
                    (i == 7 && j == 2) || (i == 7 && j == 9) || 
                    (i == 11 && j == 5))
                {
                    level1Arr[i, j] = false; // set tile to not traversable
                }
                else
                {
                    level1Arr[i, j] = true; // set tile to traversable
                }
            }
        }
    }

    /// <returns> current static array for the level </returns>
    public static bool [,] getLevelArray()
    {
        return levelMasterArray.ToArray()[GameManager.getCurrentLevel()];
    }
    /// <returns> get the current level's row size </returns>
    public static int getLevelRows()
    {
        return row[GameManager.getCurrentLevel()];
    }
    /// <returns> get the current level's column size </returns>
    public static int getLevelCols()
    {
        return col[GameManager.getCurrentLevel()];
    }
    /// <returns> gets the spawn point coordinates for the current level</returns>
    public static Tuple<int, int> getEnemySpawnPoint()
    {
        return enemySpawnPoints[GameManager.getCurrentLevel()];
    }
    /// <returns> gets the enemy's goal for the current level </returns>
    public static Tuple<int, int> getLevelGoal()
    {
        return goals[GameManager.getCurrentLevel()];
    }
}
