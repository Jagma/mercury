using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class GameProgressionManager : MonoBehaviour
{
    //private int numEnemiesLeft;
    private int numEnemiesStart;
    //private int numOfBulletsUsed;
    //private int enemiesKilled;
    //private int wallsDestroyed;
    //private int damageTaken;
    //private Time currentStageTime;
    //private Time totalTimePlayed;
    private GameObject levels;
    private GameObject[] players;
    private GameObject[] gunsUsed;
    public List<string> playerActors = new List<string>();
    public List<bool> playerDown = new List<bool>();
    private List<string> player;
    // The Game progression manager is a singleton
    public static GameProgressionManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            numEnemiesStart = 0;
        }
        else
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    public bool getPlayerDownCount()
    {
        int count = 0;
        for (int i = 0; i < playerDown.Count; i++) //loop to check if all players are down.
        {
            if (playerDown[i] == true)
            {
                count++;
            }
        }

        if (count >= playerDown.Count) //checks to see if the countDown is equal or larger than the players down count.
            return true;
        else
            return false;
    }

    public void SetPlayerDown(string playerID, bool value)
    {
        for (int i = 0; i < playerActors.Count; i++)//set player down in gameprogression manager to false.
        {
            if (playerActors[i] == playerID)
            {
                playerDown[i] = value;
                return;
            }
        }

    }

    public void setPlayerList(string players)
    {
        playerActors.Add(players);
        playerDown.Add(false);
        Debug.Log(playerActors.Count);
    }

    public void RestartLevel()
    {
        numEnemiesStart = 0;
    }

    public void GameOver()
    {
        Debug.Log("Game over.");
        SceneManager.LoadScene("GameOver");
    }

    public void Reset()
    {
        for (int i = 0; i < playerActors.Count; i++)//set player down in gameprogression manager to false.
        {
            playerDown[i] = false;
        }
        numEnemiesStart = 0;
    }

    public int getPlayerCount()
    {
        return playerActors.Count;
    }
    public void LevelComplete()
    {
        Debug.Log("Level complete.");
        //DumpData();
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
            File.AppendAllText("./DumpedData.txt",player + ", " + numEnemiesStart.ToString() + ", " + Time.timeSinceLevelLoad.ToString() + Environment.NewLine);
        else
            File.WriteAllText("./DumpedData.txt",player + ", " + numEnemiesStart.ToString() + ", " + Time.timeSinceLevelLoad.ToString() + Environment.NewLine);
    }
}
