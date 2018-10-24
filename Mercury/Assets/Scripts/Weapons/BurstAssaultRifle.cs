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
        damage = 20f;
    }

    IEnumerator BulletWaitTime()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Factory.instance.CreateBullet();
            bullet.GetComponent<Round>().setDamage(damage);
            bullet.GetComponent<Projectile>().speed *= 1;
            bullet.transform.position = transform.position + transform.right * ammoOffset;
            bullet.transform.right = transform.right;
            bullet.transform.localEulerAngles += new Vector3(0, Random.Range(-ammoRandomness, ammoRandomness), 0);
            bullet.GetComponent<Projectile>().Update();
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void Use()
    {

        base.Use();

        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * ammoOffset;
        Destroy(flash, 1);
        StartCoroutine(BulletWaitTime());


        AudioManager.instance.PlayAudio("Weapon_burstAR", 1, false);

        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }

}
