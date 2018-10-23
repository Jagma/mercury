using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcWalker : RangedEnemies
{
    protected override void Start()
    {
        base.Start();
        health = 45f;
        moveSpeed = 2f;
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
