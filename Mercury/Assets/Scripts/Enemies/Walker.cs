using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MeleeEnemies
{

    protected override void Start()
    {
        base.Start();

        health = 25;
        moveSpeed = 2f;
    }

    protected override void AimAtPlayer(Vector3 direction)
    {
        base.AimAtPlayer(direction);
    }

    protected override void FixedUpdate ()
    {
        base.FixedUpdate();
    }
}
