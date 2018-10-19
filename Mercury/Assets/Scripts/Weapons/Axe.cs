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
        ammoMax = 150;
        ammoCount = 150;
        damage = 25;
    }

    protected override void Use()
    {
        base.Use();
    }

    private void OnTriggerEnter(Collider col)
    {
        Wall wall = col.GetComponent<Wall>();
        if (wall != null)
        {
            wall.Damage(damage);
        }

        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Damage(damage);
        }
    }
}
