using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : WeaponRanged
{
    protected override void Start() {
        base.Start();

        // Stats
        cooldown = 1f;
        ammoOffset = 1f;
    }

    protected override void Use()
    {
        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * ammoOffset;
        Destroy(flash, 1);

        base.Use();
        GameObject bullet = Factory.instance.CreateRocket();
        bullet.transform.position = transform.position + transform.right * ammoOffset;
        bullet.transform.right = transform.right;
        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }
}
