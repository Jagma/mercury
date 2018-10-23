using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : Projectile {
    public override void Init() {
        base.Init();

        // Stats
        speed = 20f;
    }
    public override void Destroy() {
        base.Destroy();
        GameObject a = Factory.instance.CreateBulletHit();
        a.transform.position = transform.position;
        Destroy(a, 1f);
    }

    public void setDamage(float damageA)
    {
        damage = damageA;
    }
}