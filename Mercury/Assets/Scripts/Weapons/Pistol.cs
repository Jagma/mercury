using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponRanged {

    protected override void Start() {
        base.Start();

        // Stats
        cooldown = 0.5f;
        muzzleOffset = 0.2f;
    }

    protected override void Use() {
        base.Use();

        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * muzzleOffset;
        Destroy(flash, 1);

        GameObject bullet = Factory.instance.CreateBullet();
        bullet.GetComponent<Projectile>().speed *= 2;
        bullet.transform.position = transform.position + transform.right * muzzleOffset;
        bullet.transform.right = transform.right;
        bullet.GetComponent<Projectile>().Update();

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }
}
