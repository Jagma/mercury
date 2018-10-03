using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChest : Chest
{
    GameObject randomWeapon;
    public override void Init()
    {
        base.Init();
    }

    protected override void Use()
    {
        randomWeapon = ChooseRandomWeapon();
        Vector3 spawnPos = transform.position;
        randomWeapon.transform.position = spawnPos;
        base.Delete();
    }

    public GameObject ChooseRandomWeapon()
    {
        int percentageValue = Random.Range(0, 100);

        if (percentageValue < 50) //0-49 - common item
        {
            return Factory.instance.CreatePistol();
        }
        else if (percentageValue < 50 + 20) //50-69 - uncommen item
        {
            return Factory.instance.CreateMachineGun();
        }
        else if (percentageValue < 50 + 20 +5) //70-74 -rare item
        {
            return Factory.instance.CreateLaserRifle();
        }
        else //anything else. - extremely rare item
        {
            return Factory.instance.CreateRocketLauncher();
        }
    }

}
