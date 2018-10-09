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
        ammoMaximum = 500;
        ammoCount = 40;
    }

    protected override void Use()
    {
        base.Use();

        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * ammoOffset;
        Destroy(flash, 1); 

        GameObject bullet = Factory.instance.CreateBullet();
        bullet.transform.position = transform.position + transform.right * ammoOffset;
        bullet.transform.right = transform.right;
        bullet.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);
        bullet.GetComponent<Projectile>().Update();

        AudioManager.instance.PlayAudio("dspistol",1,false);

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);

        AudioManager.instance.PlayAudio("ric1", 1, false);
    }
}
