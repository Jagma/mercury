using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : WeaponRanged
{
    private GameObject rocket;

    protected override void Start()
    {
        base.Start();

        // Stats
        cooldown = 1f;
        ammoOffset = 1f;
        ammoMaxInventory = 7;
        ammoInventory = 7;
        ammoMax = 1;
        ammoCount = 1;
        damage = 100f;
    }

    protected override void Use()
    {
        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * ammoOffset;
        Destroy(flash, 1);
        AudioManager.instance.PlayAudio("dsrlaunc", 1, false);
        base.Use();

        rocket =  Factory.instance.CreateRocket();
        rocket.GetComponent<RPG>().setDamage(damage);
        rocket.transform.position = transform.position + transform.right * ammoOffset;
        rocket.transform.right = transform.right;
        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }

}
