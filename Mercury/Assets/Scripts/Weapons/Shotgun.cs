using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponRanged
{

    protected override void Start()
    {
        base.Start();

        // Stats
        cooldown = 0.5f;
        ammoOffset = 0.6f;
        ammoRandomness = 40f;
        ammoMaxInventory = 20;
        ammoInventory = 20;
        ammoMax = 6;
        ammoCount = 6;
        damage = 25f;
    }

    protected override void Use()
    {

        base.Use();

        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * ammoOffset;
        Destroy(flash, 1);

        for (int i = 0; i < 6; i++)
        {
            GameObject bullet = Factory.instance.CreateBullet();
            bullet.GetComponent<Round>().setDamage(damage);
            bullet.GetComponent<Projectile>().speed *= 2;
            bullet.transform.position = transform.position + transform.right * ammoOffset;
            bullet.transform.right = transform.right;
            bullet.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);
            bullet.GetComponent<Projectile>().Update();
        }

        AudioManager.instance.PlayAudio("dspistol", 1, false);

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }
}
