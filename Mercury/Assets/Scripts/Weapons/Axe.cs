using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : WeaponMelee
{
    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();

        // Stats
        cooldown = 0.5f;
        ammoMaxInventory = 0;
        ammoInventory = 0;
        ammoMax = 25;
        ammoCount = 25;
        damage = 45;
    }

    protected override void Use()
    {
        base.Use();
    }

}
