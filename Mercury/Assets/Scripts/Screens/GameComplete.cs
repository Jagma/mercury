﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameComplete : MonoBehaviour
{
    public Text titleText;
    public AnimationCurve titleCurve;
    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (titleText) {
            titleText.transform.localScale = Vector3.one * titleCurve.Evaluate(Time.time);
        }        
    }

    private void DestroyInstances()
    {
        Destroy(Game.instance);
        Destroy(Factory.instance);
    }

    public void RestartGame()
    {
        GameProgressionManager.instance.Reset();
        DestroyInstances();
        SceneManager.LoadScene("GameCampaign");
    }


    public void MainMenu()
    {
        DestroyInstances();
        GameProgressionManager.instance.Reset();
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