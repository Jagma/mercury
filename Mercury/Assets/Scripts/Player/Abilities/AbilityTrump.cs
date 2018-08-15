using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrump : Ability
{
    public override void Use()
    {
        base.Use();
        GameObject wall = Factory.instance.CreateWall();
        wall.transform.position = playerActor.transform.position + playerActor.transform.forward;
    }
}
