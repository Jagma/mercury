using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChest : Chest
{
    GameObject randomWeapon;
    GameObject randomConsumable;
    int count = 1;

    protected override void Use()
    {
        if (count >= 1)
        {
            count--;
            Debug.Log("Nomral chest opened.");
            Vector3 spawnPos = transform.position;
            randomWeapon = ChooseRandomWeapon();
            randomWeapon.transform.position = spawnPos;
            randomConsumable = ChooseRandomConsumable();
            randomConsumable.transform.position = spawnPos;
            base.Delete();
        }
    }

    public GameObject ChooseRandomWeapon()
    {
        int[] itemWeights = { 20000000, 2, 1, 2,3,1,1,20,20,20 };

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
        return null;
    }

    public GameObject ChooseRandomConsumable()
    {

        int[] itemWeights = { 3, 2,3,3,3};

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
        if (result == 1)
        {
            return Factory.instance.CreateMedpack();
        }
        if (result == 2)
        {
            return Factory.instance.CreateBeamAmmoPack();
        }
        if (result == 3)
        {
            return Factory.instance.CreateBulletAmmoPack();
        }
        if (result == 4)
        {
            return Factory.instance.CreateRocketAmmoPack();
        }
        return null;
    }

}
