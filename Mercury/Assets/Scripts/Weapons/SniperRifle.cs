using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : WeaponRanged
{
    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();
        // Stats
        cooldown = 1.2f;
        ammoOffset = 2f;
        ammoMaxInventory = 15;
        ammoInventory = 15;
        ammoMax = 5;
        ammoCount = 5;
        damage = 100f;
    }

    protected override void Use()
    {
        base.Use();

        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * ammoOffset;
        Destroy(flash, 1);

        GameObject bullet = Factory.instance.CreateSniperBullet();
        bullet.GetComponent<Round>().setDamage(damage);
        bullet.GetComponent<Projectile>().speed *= 2;
        bullet.transform.position = transform.position + transform.right * ammoOffset;
        bullet.transform.right = transform.right;
        bullet.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);
        bullet.GetComponent<Projectile>().Update();

        AudioManager.instance.PlayAudio("Weapon_sniper", .6f, false);

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }

}
