using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)) {
            PlayerInput pi = new PlayerInput("Keyboard|0001", PlayerInput.InputType.Keyboard);
            InputManager.instance.AddPlayerInput(pi);
            SceneManager.LoadScene("Game");
        }
	}
}