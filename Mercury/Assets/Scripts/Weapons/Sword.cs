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
        durability = 100f;
        damage = 5;
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
