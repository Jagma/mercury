using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cracker : RangedEnemies
{
    protected override void Start()
    {
        base.Start();
        health = 75;
        moveSpeed = 1f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void IdleMovement()
    {
        base.IdleMovement();
    }

    protected override void AimAtPlayer(Vector3 direction)
    {
        base.AimAtPlayer(direction);
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();
    }
}
