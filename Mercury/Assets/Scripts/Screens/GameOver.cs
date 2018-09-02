using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
    }

    public void RestartGame()
    {
        DestroyInstances();
        SceneManager.LoadScene("GameCOOP");
    }

    public void MainMenu()
    {
        DestroyInstances();
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void DestroyInstances()
    {
        Destroy(Game.instance);
        Destroy(Factory.instance);
        Destroy(Portal.instance);
    }
}