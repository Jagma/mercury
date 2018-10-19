using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstAssaultRifle : WeaponRanged
{

    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();
        // Stats
        cooldown = 0.75f;
        ammoOffset = 0.6f;
        ammoRandomness = 2f;
        ammoMaxInventory = 120;
        ammoInventory = 120;
        ammoMax = 31;
        ammoCount = 31;
        damage = 25f;
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

        GameObject bullet2 = Factory.instance.CreateBullet();
        bullet2.GetComponent<Round>().setDamage(damage);
        bullet2.GetComponent<Projectile>().speed *= 1.75f;
        bullet2.transform.position = transform.position + transform.right * ammoOffset;
        bullet2.transform.right = transform.right;
        bullet2.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);
        bullet2.GetComponent<Projectile>().Update();

        GameObject bullet3 = Factory.instance.CreateBullet();
        bullet3.GetComponent<Round>().setDamage(damage);
        bullet3.GetComponent<Projectile>().speed *= 1.5f;
        bullet3.transform.position = transform.position + transform.right * ammoOffset;
        bullet3.transform.right = transform.right;
        bullet3.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);
        bullet3.GetComponent<Projectile>().Update();
        AudioManager.instance.PlayAudio("BurstAR_sound_effect", 1, false);

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }

}
