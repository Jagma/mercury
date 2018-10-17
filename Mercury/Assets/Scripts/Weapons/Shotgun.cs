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
        ammoRandomness = 20f;
        ammoMaxInventory = 20;
        ammoInventory = 20;
        ammoMax = 6;
        ammoCount = 6;
        damage = 25;
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

      /*  GameObject bullet1 = Factory.instance.CreateBullet();
        GameObject bullet2 = Factory.instance.CreateBullet();
        GameObject bullet3 = Factory.instance.CreateBullet();

        bullet1.GetComponent<Round>().setDamage(damage);
        bullet2.GetComponent<Round>().setDamage(damage);
        bullet3.GetComponent<Round>().setDamage(damage);

        bullet1.transform.position = transform.position + transform.right * ammoOffset;
        bullet1.transform.right = transform.right;
        bullet1.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);

        bullet2.transform.position = transform.position + transform.right * ammoOffset;
        bullet2.transform.right = transform.right;
        bullet2.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);

        bullet3.transform.position = transform.position + transform.right * ammoOffset;
        bullet3.transform.right = transform.right;
        bullet3.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);

        bullet1.GetComponent<Projectile>().Update();
        bullet2.GetComponent<Projectile>().Update();
        bullet3.GetComponent<Projectile>().Update();*/
      
        AudioManager.instance.PlayAudio("dspistol", 1, false);

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }

    public void setDamage(int damageA)
    {
        damage = damageA;
    }
}
