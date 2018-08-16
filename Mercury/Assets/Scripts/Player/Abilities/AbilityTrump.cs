using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrump : Ability {
    private int offset = 2;
    private float abilityCooldown = 5; // Seconds cooldown

    public override void Use() {
        base.Use();
        this.SetCooldownTime(abilityCooldown);
        GameObject wall = Factory.instance.CreateWall();
        wall.transform.position = playerActor.transform.position +
            new Vector3(InputManager.instance.GetAimDirection(playerActor.model.playerID).x, 0, InputManager.instance.GetAimDirection(playerActor.model.playerID).y) * offset;
    }
}
