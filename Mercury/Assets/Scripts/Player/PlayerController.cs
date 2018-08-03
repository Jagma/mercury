using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerModel model;
    public PlayerActor actor;
	void Start () {

	}
	
	void Update () {
        actor.Move(InputManager.instance.GetMoveDirection(model.playerID));
        actor.Aim(InputManager.instance.GetAimDirection(model.playerID));
        actor.Interact(InputManager.instance.GetInteractionKey(model.playerID));
        if(InputManager.instance.GetAttack(model.playerID)) {
            actor.Attack();
        }
    }
}
