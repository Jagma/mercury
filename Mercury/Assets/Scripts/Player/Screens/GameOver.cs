using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text titleText;
    public AnimationCurve titleCurve;
    // Use this for initialization
    void Start()
    {
        AudioManager.instance.PlayAudio("Game_music_Unknown_planet", .4f, true);
    }

    void Update()
    {
        if (titleText != null)
        {
            titleText.transform.localScale = Vector3.one * titleCurve.Evaluate(Time.time);
        }
    }

    public void RestartGame()
    {
        DestroyInstances();
        AudioManager.instance.StopAllAudio();
        AudioManager.instance.PlayAudio("sfx_sounds_button5", .7f, false);
        SceneManager.LoadScene("GameCOOP");
    }

    public void MainMenu()
    {
        DestroyInstances();
        AudioManager.instance.StopAllAudio();
        AudioManager.instance.PlayAudio("sfx_sounds_button5", .7f, false);
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        AudioManager.instance.PlayAudio("sfx_sounds_button5", .7f, false);
        Application.Quit();
    }

    private void DestroyInstances()
    {
        Destroy(Game.instance);
        Destroy(Factory.instance);
        GameProgressionManager.instance.Reset();
    }

    IEnumerator ETitleJuice()
    {
        titleText.transform.localScale += new Vector3(0.1f, 0.15f, 0);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ETitleJuice());
    }
}