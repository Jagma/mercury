using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBinLaden : Ability {

    GameObject bag;
    Vector3 aimDirection, position;
    float power;
    public override void Init()
    {
        base.Init();
        
        // Stats
        cooldown = 0f;
        power = 10;
        placementOffset = 1;
    }

    protected override void Use()
    {
        Init();
        base.Use();

        //Get aim and rotate by 45*
        Vector2 aim = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        if (aim.magnitude <= 0.1)
        {
            aim = aim * 100f;
        }
        aimDirection = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(aim.x, 0, aim.y);

        //position to place object
        position = playerActor.transform.position + new Vector3(aimDirection.x, aimDirection.y, aimDirection.z) * placementOffset;
        bag = Factory.instance.CreateTNTBag();
        bag.transform.position = position;
        ThrowBag();
    }

    private void PlaceBag()
    {

    }

    private void ThrowBag()
    {
        bag.GetComponent<TNTBag>().Throw(power,playerActor.transform.position);
        
    }
}
