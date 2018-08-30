using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : WeaponRanged
{
    protected override void Start()
    {
        base.Start();

        // Stats
        cooldown = 1f;
        ammoOffset = 1f;
    }

    protected override void Use()
    {
        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * ammoOffset;
        Destroy(flash, 1);
        AudioManager.instance.PlayAudio("dsrlaunc", 1, false);
        base.Use();
        
        GameObject rocket = Factory.instance.CreateRocket();
        rocket.transform.position = transform.position + transform.right * ammoOffset;
        rocket.transform.right = transform.right;
        CameraSystem.instance.ShakePosition(-transform.right * 0.2f);
    }
}
