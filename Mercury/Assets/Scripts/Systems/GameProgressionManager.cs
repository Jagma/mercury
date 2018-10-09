using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameProgressionManager : MonoBehaviour
{
    private int numEnemiesLeft;
    private int numOfBulletsUsed;
    private int enemiesKilled;
    private int wallsDestroyed;
    private int damageTaken;
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
        numOfBulletsUsed = 0;
        enemiesKilled = 0;
        wallsDestroyed = 0;
        damageTaken = 0;
        currentStageTime = null;
        totalTimePlayed = null;
    }
	
	// Update is called once per frame
	void Update ()
    {

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
        SceneManager.LoadScene("GameOver");
    }

    public void LevelComplete()
    {
        Debug.Log("Level complete.");
        SceneManager.LoadScene("LevelComplete");
    }
    
    public void IncreaseEnemyCount()
    {
        numEnemiesLeft += 1;
    }
}
