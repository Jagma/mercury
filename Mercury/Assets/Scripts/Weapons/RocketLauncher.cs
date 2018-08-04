using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : WeaponRanged
{
    protected override void Start() {
        base.Start();

        // Stats
        cooldown = 1f;
        projectileOffset = 0.2f;
    }

    protected override void Use()
    {
        base.Use();
        GameObject bullet = Factory.instance.CreateRocket();
        bullet.transform.position = transform.position + transform.right * projectileOffset;
        bullet.transform.right = transform.right;
        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }
}
