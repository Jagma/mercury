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
            SceneManager.LoadScene("GameCOOP");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Destroy(InputManager.instance);
            Destroy(Game.instance);
            Destroy(Factory.instance);
            SceneManager.LoadScene("Menu");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //SceneManager.LoadScene("GameCOOP"); 
        }
    }

    public void RestartGame()
    {
        Destroy(Game.instance);
        Destroy(Factory.instance);
        SceneManager.LoadScene("GameCOOP");
    }

    public void ContinueGame()
    {
        Destroy(Game.instance);
        Destroy(Factory.instance);
        //SceneManager.LoadScene("Menu");
    }

    public void MainMenu()
    {
        Destroy(InputManager.instance);
        Destroy(Game.instance);
        Destroy(Factory.instance);
        SceneManager.LoadScene("Menu");
    }
}