using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponRanged {

    protected override void Start() {
        base.Start();

        // Stats
        cooldown = 0.5f;
        projectileOffset = 0.2f;
    }

    protected override void Use() {
        base.Use();
        GameObject bullet = Factory.instance.CreateBullet();
        bullet.GetComponent<Projectile>().speed *= 2;
        bullet.transform.position = transform.position + transform.right * projectileOffset;
        bullet.transform.right = transform.right;

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }
}
