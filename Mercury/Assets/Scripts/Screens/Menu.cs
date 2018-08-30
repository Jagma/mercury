using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.instance.PlayAudio("soviet-anthem", 1, false);
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)) {
            PlayerInput pi = new PlayerInput("Keyboard|0001", PlayerInput.InputType.Keyboard);
            InputManager.instance.AddPlayerInput(pi);
            AudioManager.instance.StopAudio("soviet-anthem");
            SceneManager.LoadScene("GameCOOP");
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            PlayerInput pi = new PlayerInput("Keyboard|0001", PlayerInput.InputType.Keyboard);
            InputManager.instance.AddPlayerInput(pi);
            AudioManager.instance.StopAudio("soviet-anthem");
            SceneManager.LoadScene("Multiplayer");
        }
    }
}