using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class GameProgressionManager : MonoBehaviour
{
    private int numEnemiesLeft;
    private int numEnemiesStart;
    private int numOfBulletsUsed;
    private int enemiesKilled;
    private int wallsDestroyed;
    private int damageTaken;
    private string player;
    private Time currentStageTime;
    private Time totalTimePlayed;
    private GameObject levels;
    private GameObject[] players;
    private GameObject[] gunsUsed;

    // The Game progression manager is a singleton
    public static GameProgressionManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        numEnemiesLeft = 0;
        numEnemiesStart = 0;
        numOfBulletsUsed = 0;
        enemiesKilled = 0;
        wallsDestroyed = 0;
        damageTaken = 0;
        currentStageTime = null;
        totalTimePlayed = null;
        numEnemiesStart = EnemyManager.instance.GetEnemyCount();
        Debug.Log("Enemy Count: " + numEnemiesStart);
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void setPlayer(string value)
    {
        player = value;
    }

    public void RestartLevel()
    {
        numEnemiesLeft = 0;
        numOfBulletsUsed = 0;
        enemiesKilled = 0;
        wallsDestroyed = 0;
        damageTaken = 0;
        currentStageTime = null;
    }

    public void GameOver()
    {
        Debug.Log("Game over.");
        DumpData();
        SceneManager.LoadScene("GameOver");
    }

    public void LevelComplete()
    {
        Debug.Log("Level complete.");
        DumpData();
        SceneManager.LoadScene("LevelComplete");
    }
    
    public void IncreaseEnemyCount()
    {
        numEnemiesStart += 1;
    }

    //player, 64, tyd;
    public void DumpData()
    {
        if (File.Exists("./DumpedData.txt"))
            File.AppendAllText("./DumpedData.txt", "Player: " + player + ", " + numEnemiesStart.ToString() + ", " + Time.timeSinceLevelLoad.ToString() + Environment.NewLine);
        else
            File.WriteAllText("./DumpedData.txt", "Player: " + player + ", " + numEnemiesStart.ToString() + ", " + Time.timeSinceLevelLoad.ToString() + Environment.NewLine);
 
        Debug.Log("Data dumped to text file.");
    }
}
