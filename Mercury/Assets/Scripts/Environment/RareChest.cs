using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareChest : NormalChest
{
    protected override GameObject GetRandomItem() {
        int result = Random.Range(0, 5);

        if (result == 0) {
            return Factory.instance.CreateLaserPistol();
        }
        if (result == 1) {
            return Factory.instance.CreateLaserRayGun();
        }
        if (result == 2) {
            return Factory.instance.CreateFlamethrower();
        }
        if (result == 3) {
            return Factory.instance.CreateLaserMachineGun();
        }
        if (result == 4) {
            return Factory.instance.CreateShotgun();
        }
        return null;
    }
}
