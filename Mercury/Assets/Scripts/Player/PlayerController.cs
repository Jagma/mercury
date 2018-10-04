using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{  
    public PlayerActor actor;

	void Update ()
    {
        actor.Move(InputManager.instance.GetMoveDirection(actor.model.playerID));
        actor.Aim(InputManager.instance.GetAimDirection(actor.model.playerID));

        if (InputManager.instance.GetInteractPressed(actor.model.playerID))
        {
            actor.Interact();
        }
        if(InputManager.instance.GetAttack(actor.model.playerID))
        {
            actor.Attack();
        }
        if (InputManager.instance.GetUseAbility(actor.model.playerID))
        {
            actor.UseAbility();
        }
        if (InputManager.instance.GetSwitchWeaponsPressed(actor.model.playerID))
        {
            actor.SwitchWeapons();
        }
    }
}
