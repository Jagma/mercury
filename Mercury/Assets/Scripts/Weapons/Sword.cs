using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponMelee
{

    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();
        // Stats
        cooldown = 0.5f;
        ammoMaxInventory = 0;
        ammoInventory = 0;
        ammoMax = 100;
        ammoCount = 100;
        damage = 15f;
    }

    protected override void Use()
    {
        base.Use();
    }
}
