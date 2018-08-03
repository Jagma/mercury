using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponRanged {

    protected override void Start() {
        base.Start();

        // Stats
        cooldown = 0.1f;
    }

    protected override void Use() {
        base.Use();
        GameObject bullet = Factory.instance.CreateBullet();
        bullet.transform.position = transform.position + transform.right * 0.2f;
        bullet.transform.right = transform.right;

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }
}
