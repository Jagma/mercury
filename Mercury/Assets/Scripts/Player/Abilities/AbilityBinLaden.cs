using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBinLaden : Ability {

    GameObject bag;
    public override void Init()
    {
        base.Init();
        
        // Stats
        cooldown = 0f;
    }

    protected override void Use()
    {
        base.Use();
        ThrowBag();
        //PlaceBag();
    }

    private void PlaceBag()
    {
        Vector3 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);

        bag = Factory.instance.CreateTNTBag();
        bag.transform.position = playerActor.transform.position + new Vector3(aimDirection.x, 0, aimDirection.y) * 2f;
        bag.transform.right = new Vector3(aimDirection.x, 0, aimDirection.y);
        bag.GetComponent<Projectile>().Update();
    }

    private void ThrowBag()
    {
        Vector3 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);

        bag = Factory.instance.CreateTNTBag();
        //bag.GetComponent<Projectile>().speed += 1f;
        bag.transform.position = playerActor.transform.position + new Vector3(aimDirection.x, 0, aimDirection.y) * 1f;
        bag.GetComponent<TNTBag>().Move(aimDirection);
        bag.transform.right += new Vector3(aimDirection.x, 0, aimDirection.y);
        bag.GetComponent<Projectile>().Update();
    }
}
