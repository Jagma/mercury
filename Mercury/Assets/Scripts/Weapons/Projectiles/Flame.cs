using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Projectile
{
    public override void Init()
    {
        base.Init();
        // Stats
        speed = 10f;
        damage = 100;
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    private void OnTriggerEnter(Collider col)
    {

    }
}
