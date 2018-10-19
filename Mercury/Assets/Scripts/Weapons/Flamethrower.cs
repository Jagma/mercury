using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : WeaponRanged
{
    private GameObject flame;

    protected override void Start()
    {
        base.Start();

        // Stats
        cooldown = 1f;
        ammoOffset = 1f;
        ammoMaxInventory = 200;
        ammoInventory = 200;
        ammoMax = 100;
        ammoCount = 100;
        damage = 50f;
    }

    protected override void Use()
    {
        base.Use();

        flame = Factory.instance.CreateFlame();
    }

}
