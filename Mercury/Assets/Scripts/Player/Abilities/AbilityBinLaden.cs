using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBinLaden : Ability {

    GameObject bag;
    Vector3 aimDirection, position;
    public override void Init()
    {
        base.Init();
        
        // Stats
        cooldown = 0f;
    }

    protected override void Use()
    {
        base.Use();

        AudioManager.instance.PlayAudio("abra", 1, false);

        //Get aim and rotate by 45*
        Vector2 aim = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        if (aim.magnitude <= 0.1)
        {
            aim = aim * 100f;
        }
        aimDirection = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(aim.x, 0, aim.y);

        //position to place object
        position = playerActor.transform.position + new Vector3(aimDirection.x, aimDirection.y, aimDirection.z) * placementOffset;


        ThrowBag();
        //PlaceBag();
        
    }

    private void PlaceBag()
    {

        bag = Factory.instance.CreateTNTBag();
        bag.transform.position = position;
        bag.transform.right = aimDirection;
        bag.GetComponent<Projectile>().Update();
    }

    private void ThrowBag()
    {
        bag = Factory.instance.CreateTNTBag();
        bag.transform.position = position;
        bag.GetComponent<TNTBag>().Move(aimDirection);
        bag.transform.right = aimDirection;
        bag.GetComponent<Projectile>().Update();
    }
}
