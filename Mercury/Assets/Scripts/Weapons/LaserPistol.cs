using UnityEngine;
using System.Collections;

public class LaserPistol : WeaponRanged
{
    GameObject beam;

    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();
        damage = 45f;
        beam = Factory.instance.CreateBeamPurple();
        beam.transform.parent = transform;
        beam.SetActive(false);
        // Stats
        cooldown = 1f;
        ammoOffset = 0.25f;
        ammoMaxInventory = 10;
        ammoInventory = 10;
        ammoMax = 10;
        ammoCount = 10;
    }

    protected override void Use()
    {
        base.Use();
        beam.transform.position = transform.position + transform.right * ammoOffset;
        beam.transform.right = transform.right;

        AudioManager.instance.PlayAudio("sfx_wpn_laser 10", 0.95f, false);
        beam.SetActive(true);
        framesSinceUse = 0;
    }

    int framesSinceUse = 0;
    protected override void Update()
    {
        base.Update();
        // This is to disable the beam once the weapon stops being used
        framesSinceUse++;
        if (framesSinceUse > 4)
        {
            beam.SetActive(false);
        }
    }
}
