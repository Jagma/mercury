using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWalker : Enemy
{
    protected override void Start()
    {
        base.Start();

        health = 5000;
        moveSpeed = 2f;
    }

}
