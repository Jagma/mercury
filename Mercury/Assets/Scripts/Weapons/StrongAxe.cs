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
        int weaponIndex = UnityEngine.Random.Range(0, 4);
        switch (weaponIndex)
        {
            case 0:
                AudioManager.instance.PlayAudio("sword1", 1, false);
                break;
            case 1:
                AudioManager.instance.PlayAudio("sword2", 1, false);
                break;
            case 2:
                AudioManager.instance.PlayAudio("slash1", 1, false);
                break;
            case 3:
                AudioManager.instance.PlayAudio("ax1", 1, false);
                break;
        }
        base.Use();
    }

}
