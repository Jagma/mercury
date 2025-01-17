﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAxe : WeaponMelee
{
    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();

        // Stats
        cooldown = 0.5f;
        ammoMaxInventory = 0;
        ammoInventory = 0;
        ammoMax = 50;
        ammoCount = 50;
        damage = 20f;
    }

    protected override void Use()
    {
        base.Use();
    }

}
