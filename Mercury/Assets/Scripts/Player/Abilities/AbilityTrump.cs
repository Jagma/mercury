using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrump : Ability {
    public override void Init() {
        base.Init();

        // Stats
        cooldown = 5f;
    }

    protected override void Use() {
        base.Use();        

        GameObject wall = Factory.instance.CreateWall();
        wall.GetComponent<Wall>().health = int.MaxValue;
        Vector3 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        wall.transform.position = playerActor.transform.position + new Vector3(aimDirection.x, 0, aimDirection.y) * 1f;

        GameObject.Destroy(wall, 10f);
    }
}
