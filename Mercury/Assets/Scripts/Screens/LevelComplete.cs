using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{
    public Text titleText;
    public AnimationCurve titleCurve;
    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        titleText.transform.localScale = Vector3.one * titleCurve.Evaluate(Time.time);
    }

    private void DestroyInstances()
    {
        Destroy(Game.instance);
        Destroy(Factory.instance);
    }

    public void RestartGame()
    {
        GameProgressionManager.instance.RestartLevel();
        DestroyInstances();
        SceneManager.LoadScene("GameCOOP");
    }

    public void MainMenu()
    {
        DestroyInstances();
        SceneManager.LoadScene("Menu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator ETitleJuice()
    {
        titleText.transform.localScale += new Vector3(0.1f, 0.15f, 0);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ETitleJuice());
    }
}