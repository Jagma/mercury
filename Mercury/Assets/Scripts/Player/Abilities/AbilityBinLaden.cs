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
        Vector2 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        Vector3 normalizedAim = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(aimDirection.x, 0, aimDirection.y);
        Vector3 position = playerActor.transform.position + new Vector3(normalizedAim.x, normalizedAim.y, normalizedAim.z) * 1f;

        bag = Factory.instance.CreateTNTBag();
        bag.transform.position = position;
        bag.transform.right = normalizedAim;
        bag.GetComponent<Projectile>().Update();
    }

    private void ThrowBag()
    {
        Vector2 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        Vector3 normalizedAim = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(aimDirection.x, 0, aimDirection.y);
        Vector3 position = playerActor.transform.position + new Vector3(normalizedAim.x, normalizedAim.y, normalizedAim.z) * 1f;

        bag = Factory.instance.CreateTNTBag();
        //bag.GetComponent<Projectile>().speed += 1f;
        bag.transform.position = position;
        bag.GetComponent<TNTBag>().Move(normalizedAim);
        bag.transform.right = normalizedAim;
        bag.GetComponent<Projectile>().Update();
    }
}
