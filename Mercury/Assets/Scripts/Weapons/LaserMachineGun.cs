using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMachineGun : WeaponRanged
{

    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();
        // Stats
        cooldown = 0.1f;
        ammoOffset = 0f;
        ammoRandomness = 10f;
        ammoMaxInventory = 500;
        ammoInventory = 500;
        ammoMax = 40;
        ammoCount = 40;
        damage = 15f;
    }
    protected override void Use()
    {
        base.Use();

        GameObject flash = Factory.instance.CreateLaserMuzzleFlash();
        flash.transform.position = transform.position + transform.right* 0.7f;
        Destroy(flash, 1);

        GameObject bullet = Factory.instance.CreateLaserBullet();
        bullet.GetComponent<Round>().setDamage(damage);
        bullet.GetComponent<Projectile>().speed *= 2;
        bullet.transform.position = transform.position + transform.right * ammoOffset;
        bullet.transform.right = transform.right;
        bullet.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);
        bullet.GetComponent<Projectile>().Update();
        AudioManager.instance.PlayAudio("Laser Gun Sound Effect", 1f, false);
        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }

}
