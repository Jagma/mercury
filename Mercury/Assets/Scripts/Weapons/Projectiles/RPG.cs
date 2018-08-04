using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : Projectile
{
    public override void Init()
    {
        base.Init();

        // Stats
        speed = 10f;
        damage = 5;
    }

    public override void Destroy()
    {
        base.Destroy();
        GameObject a = Factory.instance.CreateBulletHit();
        a.transform.position = transform.position;
        Destroy(a, 1f);
    }
}
