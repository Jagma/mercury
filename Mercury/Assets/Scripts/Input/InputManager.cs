using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour { 
    // The input manager contains all devices that have been assigned to a player.
    // (All devices that will be used in the game)

    public static InputManager instance;
    Dictionary<string, PlayerInput> playerInputs = new Dictionary<string, PlayerInput>();

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void AddPlayerInput (PlayerInput pi) {
        playerInputs.Add(pi.playerID, pi); 
    }

    public void RemovePlayerInput (PlayerInput pi) {
        playerInputs.Remove(pi.playerID);
    }

    public Dictionary<string, PlayerInput> GetPlayerInputDictionary () {
        return playerInputs;
    }

    // ** Methods :
    public bool GetFire (string playerID) {
        return playerInputs[playerID].GetFire();
    }

    public Vector2 GetMoveDirection (string playerID) {
        return playerInputs[playerID].GetMoveDirection();
    }
}
