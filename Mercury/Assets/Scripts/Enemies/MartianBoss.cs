using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class 
    MartianBoss : Enemy
{
    private Timer timer;
    private PlayerActor closestPlayerActor = null;
    int i = 0;
    protected override void Start()
    {
        base.Start();

        health = 1000;
        moveSpeed = 15f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

    }
}
