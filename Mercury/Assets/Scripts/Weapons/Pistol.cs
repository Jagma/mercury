using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponRanged
{

    // Stats
    new float cooldown = 0.5f;
    new float ammoOffset = 0.2f;
    new int ammoMaxInventory = 60;
    new int ammoInventory = 60;
    new int ammoMax = 15;
    new int ammoCount = 15;
    new float damage = 5f;

    protected override void Start()
    {
        base.Start();
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

        AudioManager.instance.PlayAudio("dspistol", 1f, false);

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }

}
