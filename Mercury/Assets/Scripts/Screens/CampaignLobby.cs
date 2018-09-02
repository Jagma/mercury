using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignLobby : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return)) {
            PlayerInput pi = new PlayerInput("Keyboard|0001", PlayerInput.InputType.Keyboard);
            InputManager.instance.AddPlayerInput(pi);
            AudioManager.instance.StopAudio("soviet-anthem");
            SceneManager.LoadScene("GameCOOP");
        }
    }
}
