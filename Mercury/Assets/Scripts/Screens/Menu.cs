﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour {
    
    public EventSystem eventSystem;

    GameObject previousSelection;
    void Start () {
        AudioManager.instance.PlayAudio("soviet-anthem", 1, false);
    }

    private void Update() {
        // This ensures that there is always a selected UI component
        if (eventSystem.currentSelectedGameObject == null) {
            eventSystem.SetSelectedGameObject(previousSelection);
        }
        previousSelection = eventSystem.currentSelectedGameObject;
    }

    public void Select(GameObject go) {
        eventSystem.SetSelectedGameObject(go);
    }

    public void NavigateCampaign () {
        SceneManager.LoadScene("CampaignLobby");
    }

    public void NavigateTeamDeathmatch () {
        // SceneManager.LoadScene("");
        Debug.LogError("Not implemented");
    }

    public void NavigateExit () {
        Application.Quit();
    }
}