using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChest : Chest
{

    protected override GameObject GetRandomItem()
    {
        int result;
        if (ProgressionState.environmentName == "Mars")
        {
            result = Random.Range(0, 2);
            if (result == 0)
                return Factory.instance.CreatePistol();
            if (result == 1)
                return Factory.instance.CreateMachineGun();
            if (result == 2)
                return Factory.instance.CreateSniperRifle();
        }

        if (ProgressionState.environmentName == "Venus")
        {
            result = Random.Range(3, 6);
            if (result == 3)
                return Factory.instance.CreateRocketLauncher();
            if (result == 4)
                return Factory.instance.CreateShotgun();
            if (result == 6)
                return Factory.instance.CreateSword();
        }

        if (ProgressionState.environmentName == "Mercury")
        {
            result = Random.Range(5, 8);
            if (result == 5)
                return Factory.instance.CreateLaserPistol();
            if (result == 7)
                return Factory.instance.CreateStrongAxe();
            if (result == 8)
                return Factory.instance.CreateWeakAxe();
        }
      
        return null;
    }
}
