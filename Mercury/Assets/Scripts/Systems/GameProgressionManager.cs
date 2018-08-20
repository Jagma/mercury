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
        //else
            //Destroy(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void GameOver()
    {
        //GameCOOP scene is index 0
        SceneManager.UnloadSceneAsync("GameCOOP");
        Destroy(InputManager.instance);
        Destroy(Game.instance);
        Destroy(Factory.instance);
        SceneManager.LoadScene("GameOver");
    }
}
