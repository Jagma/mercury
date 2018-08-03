using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerModel model;
    public PlayerActor actor;

	void Update () {
        actor.Move(InputManager.instance.GetMoveDirection(model.playerID));
        actor.Aim(InputManager.instance.GetAimDirection(model.playerID));

        if (InputManager.instance.GetInteractPressed(model.playerID)) {
            actor.Interact();
        }
        if(InputManager.instance.GetAttack(model.playerID)) {
            actor.Attack();
        }
    }
}
