using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAxe : WeaponMelee
{
    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();

        // Stats
        cooldown = 0.5f;
        ammoMaxInventory = 0;
        ammoInventory = 0;
        ammoMax = 20;
        ammoCount = 20;
        damage = 45f;
    }

    protected override void Use()
    {
        base.Use();
    }

}
