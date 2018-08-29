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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DestroyInstances();
            SceneManager.LoadScene("GameCOOP");
        }
    }

    public void RestartGame()
    {
        DestroyInstances();
        SceneManager.LoadScene("GameCOOP");
    }

    private void DestroyInstances()
    {
        Destroy(Game.instance);
        Destroy(Factory.instance);
        Destroy(Portal.instance);
    }
}