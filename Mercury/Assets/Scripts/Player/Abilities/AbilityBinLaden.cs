using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBinLaden : Ability {

    public override void Init()
    {
        base.Init();

        // Stats
        cooldown = 0f;
    }

    protected override void Use()
    {
        base.Use();
        Vector3 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        GameObject bag = Factory.instance.CreateTNTBag();

        bag.GetComponent<Projectile>().speed *= 1f;
        bag.transform.position = playerActor.transform.position + new Vector3(aimDirection.x, 0, aimDirection.y) * 3f;
        bag.transform.right = new Vector3(aimDirection.x, 0, aimDirection.y);
        bag.GetComponent<Projectile>().Update();
    }
}
