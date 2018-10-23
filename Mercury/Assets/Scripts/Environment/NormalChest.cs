using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChest : Chest
{

    protected override GameObject GetRandomItem()
    {
        int result = Random.Range(0, 10);
        //Weapons.
        if (result == 0)
        {
            return Factory.instance.CreatePistol();
        }
        if (result == 1)
        {
            return Factory.instance.CreateMachineGun();
        }
        if (result == 2)
        {
            return Factory.instance.CreateSniperRifle();
        }
        if (result == 3)
        {
            return Factory.instance.CreateRocketLauncher();
        }
        if (result == 4)
        {
            return Factory.instance.CreateShotgun();
        }
        if (result == 5)
        {
            return Factory.instance.CreateLaserPistol();
        }
        if (result == 6)
        {
            return Factory.instance.CreateSword();
        }

        if (result == 7)
        {
            return Factory.instance.CreateAxe();
        }

        if (result == 8)
        {
            return Factory.instance.CreateSpear();
        }
        // Consumables
        if (result == 9)
        {
            return Factory.instance.CreateMedkit();
        }
        if (result == 10)
        {
            return Factory.instance.CreateMedpack();
        }
        return null;
    }
}
