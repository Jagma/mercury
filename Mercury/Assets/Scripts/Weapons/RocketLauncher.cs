using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : WeaponRanged
{
    protected override void Start() {
        base.Start();

        // Stats
        cooldown = 1f;
        muzzleOffset = 1f;
    }

    protected override void Use()
    {
        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * muzzleOffset;
        Destroy(flash, 1);

        base.Use();
        GameObject bullet = Factory.instance.CreateRocket();
        bullet.transform.position = transform.position + transform.right * muzzleOffset;
        bullet.transform.right = transform.right;
        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }
}
