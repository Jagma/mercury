using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkInput : MonoBehaviour {
    private void Awake() {
        PlayerInput pi = new PlayerInput("Keyboard|0001", PlayerInput.InputType.Keyboard);
        InputManager.instance.AddPlayerInput(pi);
    }
    void Update() {
        // Movement
        Vector2 moveDir = InputManager.instance.GetMoveDirection("Keyboard|0001");
        NetworkModel.instance.SetModel(GameDeathmatch.clientUniqueID + "Input_Move", NetworkVector3.FromVector3(moveDir));

        // Look
        Vector2 aimDir = InputManager.instance.GetAimDirection("Keyboard|0001");
        NetworkModel.instance.SetModel(GameDeathmatch.clientUniqueID + "Input_Look", NetworkVector3.FromVector3(aimDir));

        // Attack
        bool attack = InputManager.instance.GetAttack("Keyboard|0001");
        NetworkModel.instance.SetModel(GameDeathmatch.clientUniqueID + "Input_Attack", attack);
    }
}