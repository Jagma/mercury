using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : WeaponRanged
{
    protected override void Use()
    {
        base.Use();
        GameObject bullet = Factory.instance.CreateRocketBullet();
        bullet.transform.position = transform.position + transform.right * 0.2f;
        bullet.transform.right = transform.right;
    }
}
