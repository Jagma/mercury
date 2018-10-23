using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareChest : Chest
{
    protected override void Use()
    {
        base.Use();
    }

    protected override GameObject GetRandomItem()
    {
        itemWeights = new int[] { 15, 30, 5, 35, 40};
        base.GetRandomItem();

        if (result == 0)
        {
            return Factory.instance.CreateLaserPistol();
        }
        if (result == 1)
        {
            return Factory.instance.CreateLaserRayGun();
        }
        if (result == 2)
        {
            return Factory.instance.CreateFlamethrower();
        }
        if (result == 3)
        {
            return Factory.instance.CreateLaserMachineGun();
        }
        if (result == 4)
        {
            return Factory.instance.CreateShotgun();
        }
        return null;
    }
}
