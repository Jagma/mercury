using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChest : Chest
{
    GameObject randomWeapon;
    GameObject randomHealthPack;
    int count = 1;

    protected override void Use()
    {
        if (count >= 1)
        {
            count--;
            Debug.Log("Chest opened.");
            Vector3 spawnPos = transform.position;
            randomWeapon = ChooseRandomWeapon();
            randomWeapon.transform.position = spawnPos;
            randomHealthPack = ChooseRandomHealthPack();
            randomHealthPack.transform.position = spawnPos;
            base.Delete();
        }
    }

    public GameObject ChooseRandomWeapon()
    {
        int[] itemWeights = { 2, 2, 1, 2,3,1,1 };

        int total = 0;
        for (int i=0; i < itemWeights.Length; i ++)
        {
            total += itemWeights[i];
        }

        int random = Random.Range(0, total);

        total = 0;
        int result = 0;
        for (int i=0; i < itemWeights.Length; i ++)
        {
            total += itemWeights[i];
            if (random <= total) {
                result = i;
                break;
            }
        }

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
            return Factory.instance.CreateLaserRifle();
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
       /* if (result == 6)
        {
            return Factory.instance.CreateFlamethrower();
        }*/
        return null;
    }

    public GameObject ChooseRandomHealthPack()
    {

        int[] itemWeights = { 3, 2};

        int total = 0;
        for (int i = 0; i < itemWeights.Length; i++)
        {
            total += itemWeights[i];
        }

        int random = Random.Range(0, total);

        total = 0;
        int result = 0;
        for (int i = 0; i < itemWeights.Length; i++)
        {
            total += itemWeights[i];
            if (random < total)
            {
                result = i;
                break;
            }
        }

        if (result == 0)
        {
            return Factory.instance.CreateMedkit();
        }
        else
        {
            return Factory.instance.CreateMedpack();
        }
    }

}
