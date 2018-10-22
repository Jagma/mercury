using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : WeaponMelee
{
    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();

        // Stats
        cooldown = 0.5f;
        ammoMaxInventory = 0;
        ammoInventory = 0;
        ammoMax = 30;
        ammoCount = 30;
        damage = 35;
    }

    protected override void Use()
    {
        base.Use();
    }

}
