using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : WeaponRanged
{
    GameObject flame;

    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();
        // Stats

        cooldown = 1f;
        ammoOffset = 1f;
        ammoMaxInventory = 200;
        ammoInventory = 200;
        ammoMax = 100;
        ammoCount = 100;
        damage = 1f;
    }

    protected override void Use()
    {
        base.Use();

    }

    int framesSinceUse = 0;
    protected override void Update()
    {
        base.Update();
        // This is to disable the flame once the weapon stops being used
        framesSinceUse++;
        if (framesSinceUse > 2)
        {
            flame.SetActive(false);
        }
    }


}
