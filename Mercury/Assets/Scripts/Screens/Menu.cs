using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.instance.PlayAudio("soviet-anthem", 1, false);
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