using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponMelee
{

    protected override void Start()
    {
        base.Start();

        // Stats
        cooldown = 0.5f;
        ammoMaxInventory = 0;
        ammoInventory = 0;
        ammoMax = 100;
        ammoCount = 100;
        damage = 10;
    }

    protected override void Use()
    {
        base.Use();
    }

    public void setDamage(int damageA)
    {
        damage = damageA;
    }
}
