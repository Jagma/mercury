﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : WeaponRanged
{

    protected override void InitWeaponStats()
    {
        // Stats
        base.InitWeaponStats();
        cooldown = 0.75f;
        ammoOffset = 0.2f;
        ammoMaxInventory = 18;
        ammoInventory = 18;
        ammoMax = 6;
        ammoCount = 6;
        damage = 35f;
    }

    protected override void Use()
    {
        base.Use();

        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * ammoOffset;
        Destroy(flash, 1);

        GameObject bullet = Factory.instance.CreateBullet();
        bullet.GetComponent<Round>().setDamage(damage);
        bullet.GetComponent<Projectile>().speed *= 2;
        bullet.transform.position = transform.position + transform.right * ammoOffset;
        bullet.transform.right = transform.right;
        bullet.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);
        bullet.GetComponent<Projectile>().Update();

        AudioManager.instance.PlayAudio("Weapon_revolver", 1f, false);

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }

}
