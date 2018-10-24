using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Projectile {
    public override void Init() {
        base.Init();

        // Stats
        damage = 20f;
        speed = 4f;
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

    private void OnTriggerEnter(Collider collider)
    {
        PlayerActor player = collider.GetComponent<PlayerActor>();
        if(player != null)
        {
            player.Damage(damage);
        }
    }
}