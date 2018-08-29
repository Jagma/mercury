using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameProgressionManager.instance.RestartLevel();
            DestroyInstances();
            SceneManager.LoadScene("GameCOOP");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            DestroyInstances();
            Destroy(InputManager.instance);
            SceneManager.LoadScene("Menu");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //SceneManager.LoadScene("GameCOOP"); 
        }
    }

    private void DestroyInstances()
    {
        Destroy(Game.instance);
        Destroy(Factory.instance);
        Destroy(Portal.instance);
    }

    public void RestartGame()
    {
        GameProgressionManager.instance.RestartLevel();
        DestroyInstances();
        SceneManager.LoadScene("GameCOOP");
    }

    public void ContinueGame()
    {
        DestroyInstances();
        //SceneManager.LoadScene("Menu");
    }

    public void MainMenu()
    {
        DestroyInstances();
        SceneManager.LoadScene("Menu");
    }
}