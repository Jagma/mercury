using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class MalfeasanceBoss : Enemy
{
    private float cooldown;
    private float damage;
    float lineOfSight;
    float knockBack;
    float lastActivatedTime;
    bool isDashing = false;

    Rigidbody rigidbody;

    protected override void Start()
    {
        base.Start();
        health = 1000;
        moveSpeed = 10f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }


}

