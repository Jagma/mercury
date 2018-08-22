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
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void GameOver()
    {
        //GameCOOP scene is index 1 on current build
        SceneManager.UnloadSceneAsync("GameCOOP");
        Destroy(InputManager.instance);
        Destroy(Game.instance);
        Destroy(Factory.instance);
        SceneManager.LoadScene("GameOver");
    }

    public void LevelComplete()
    {
        Debug.Log("Level complete.");
        //GameCOOP scene is index 1 on current build
        /*SceneManager.UnloadSceneAsync("GameCOOP");
        Destroy(InputManager.instance);
        Destroy(Game.instance);
        Destroy(Factory.instance);
        SceneManager.LoadScene("GameOver");*/
    }
    public void EnemyDead()
    {
        numEnemiesLeft -= 1;
        if (numEnemiesLeft == 0)
        {
            GameObject portal = Factory.instance.CreatePortal();

            Vector3 playerPos = PlayerActor.instance.transform.position;
            Vector3 playerDirection = PlayerActor.instance.transform.forward;
            Quaternion playerRotation = PlayerActor.instance.transform.rotation;
            float spawnDistance = 10;  
            Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
            portal.transform.position = spawnPos;

            Debug.Log("portal spawned.");
        }
    }
    
    public void IncreaseEnemyCount()
    {
        numEnemiesLeft += 1;
    }
}
