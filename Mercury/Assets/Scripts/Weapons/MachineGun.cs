using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponRanged
{
  
    protected override void Start()
    {
        base.Start();

        // Stats
        cooldown = 0.1f;
        ammoOffset = 0.6f;
        ammoRandomness = 10f;
        ammoMaxInventory = 500;
        ammoInventory = 500;
        ammoMax = 40;
        ammoCount = 40;
        damage = 10f;
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
        AudioManager.instance.PlayAudio("sfx_wpn_machinegun_loop2", 1, false);
    }

}
