using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// The Game progression manager is a singleton
public class GameProgressionManager : MonoBehaviour
{
    private int numEnemiesLeftLevel;
    private int numEnemiesStartLevel;
    private int numOfBulletsUsedTotal;
    private int numOfBulletsUsedLevel;
    private int enemiesKilledTotal;
    private int enemiesKilledLevel;
    private int wallsDestroyedLevel;
    private int wallsDestroyedTotal;

    private float damageTakenTotal;
    private float damageTakenLevel;

    private Time currentStageTime;
    private Time totalTimePlayed;

    private GameObject[] levels;
    private GameObject[] players;
    private GameObject[] gunsUsed;

    public List<string> playerIDList = new List<string>();
    public List<bool> playerDown = new List<bool>();


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

    #region Init
    // Use this for initialization
    void Start()
    {
        numOfBulletsUsedTotal = 0;
        numOfBulletsUsedLevel = 0;
        enemiesKilledTotal = 0;
        enemiesKilledLevel = 0;
        wallsDestroyedTotal = 0;
        wallsDestroyedLevel = 0;
        damageTakenTotal = 0;
        damageTakenLevel = 0;
        currentStageTime = null;
        totalTimePlayed = null;
        numEnemiesLeftLevel = 0;
        numEnemiesStartLevel = 0;
    }

    public void SetPlayerList(string players)
    {
        playerIDList.Add(players);
        playerDown.Add(false);
    }
    #endregion

    #region LevelComplete/Restart

    public void RestartLevel()
    {
        numOfBulletsUsedLevel = 0;
        enemiesKilledLevel = 0;
        wallsDestroyedLevel = 0;
        damageTakenLevel = 0;
        currentStageTime = null;
        numEnemiesLeftLevel = 0;
        numEnemiesStartLevel = 0;
    }

    public void LevelComplete()
    {
        RestartLevel();
        SceneManager.LoadScene("LevelComplete");
    }

    #endregion

    #region Game Updates

    public int getPlayerCount()
    {
        return playerIDList.Count;
    }

    public void IncreaseEnemyKills()
    {
        enemiesKilledLevel += 1;
    }

    public void IncreaseWallsDestroyed()
    {
        wallsDestroyedLevel += 1;
    }

    public void IncreaseBulletAmountUsed()
    {
        numOfBulletsUsedLevel += 1;
    }

    public void IncreaseEnemyCount()
    {
        numEnemiesStartLevel += 1;
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
        for (int i = 0; i < playerIDList.Count; i++)//set player down in gameprogression manager to false.
        {
            if (playerIDList[i] == playerID)
            {
                playerDown[i] = value;
                return;
            }
        }

    }
    #endregion

    #region Database Updating
    void SendDataServer()
    {
        //send data to database.
    }
    #endregion

    #region Game Over
    public void GameOver()
    {
        SetFinalValues();
        SceneManager.LoadScene("GameOver");
    }

    void SetFinalValues()
    {
        numOfBulletsUsedTotal += numOfBulletsUsedLevel;
        enemiesKilledTotal += enemiesKilledLevel;
        wallsDestroyedTotal += wallsDestroyedLevel;
        damageTakenTotal += damageTakenLevel;
        //totalTimePlayed += currentStageTime;      
    }


    public void Reset()
    {
        SendDataServer();
        Start(); //reset variables.
        for (int i = 0; i < playerIDList.Count; i++)//set player down in gameprogression manager to false.
        {
            playerDown[i] = false;
        }
    }

    #endregion

}
