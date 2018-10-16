using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{ 
    // The input manager contains all devices that have been assigned to a player.
    // (All devices that will be used in the game)

    public static InputManager instance;
    Dictionary<string, PlayerInput> playerInputDictionary = new Dictionary<string, PlayerInput>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void AddPlayerInput (PlayerInput pi)
    {
        playerInputDictionary.Add(pi.playerID, pi); 
    }

    public void RemovePlayerInput (PlayerInput pi)
    {
        playerInputDictionary.Remove(pi.playerID);
    }

    public Dictionary<string, PlayerInput> GetPlayerInputDictionary ()
    {
        return playerInputDictionary;
    }

    public PlayerInput GetPlayerInput (string playerID) {
        if(playerInputDictionary.ContainsKey(playerID)) {
            return playerInputDictionary[playerID];
        }

        return null;
    }

    // ** Methods :
    public bool GetUpPressed (string playerID) {
        return playerInputDictionary[playerID].GetUpPressed(playerID);
    }

    public bool GetDownPressed(string playerID) {
        return playerInputDictionary[playerID].GetDownPressed(playerID);
    }

    public bool GetSelectPressed(string playerID) {
        return playerInputDictionary[playerID].GetSelectPressed(playerID);
    }

    public bool GetBackPressed(string playerID) {
        return playerInputDictionary[playerID].GetBackPressed(playerID);
    }

    public Vector2 GetMoveDirection(string playerID)
    {
        return playerInputDictionary[playerID].GetMoveDirection();
    }

    public Vector2 GetAimDirection(string playerID)
    {
        return playerInputDictionary[playerID].GetAimDirection();
    }

    public bool GetAttack(string playerID)
    {
        return playerInputDictionary[playerID].GetAttack();
    }

    public bool GetInteract(string playerID)
    {
        return playerInputDictionary[playerID].GetInteract();
    }

    public bool GetInteractPressed(string playerID)
    {
        return playerInputDictionary[playerID].GetInteractPressed();
    }

    public bool GetSwitchWeaponsPressed(string playerID)
    {
        return playerInputDictionary[playerID].GetSwitchWeaponsPressed();
    }

    public bool GetUseAbility(string playerID)
    {
        return playerInputDictionary[playerID].GetUseAbilityPressed();
    }

    public bool ToggleGodMode(string playerID)
    {
        return playerInputDictionary[playerID].toggleGodMode();
    }
}
