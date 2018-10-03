using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChest : Chest
{
    GameObject randomWeapon;
    protected override void Use()
    {
        Debug.Log("Chest opened.");
        Vector3 spawnPos = transform.position;
        randomWeapon = ChooseRandomWeapon();
        randomWeapon.transform.position = spawnPos;
        base.Delete();
    }

    public GameObject ChooseRandomWeapon()
    {
        int percentageValue = Random.Range(0, 100);

        if (percentageValue < 50) //0-49
        {
            return Factory.instance.CreatePistol();
        }
        else if (percentageValue < 50 + 20) //50-69
        {
            return Factory.instance.CreateMachineGun();
        }
        else if (percentageValue < 50 + 20 +5) //70-74 
        {
            return Factory.instance.CreateLaserRifle();
        }
        else //anything else.
        {
            return Factory.instance.CreateRocketLauncher();
        }
    }

}
