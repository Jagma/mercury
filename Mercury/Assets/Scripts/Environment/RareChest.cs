using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareChest : Chest
{
    GameObject randomAmmoPack;
    System.Random ran = new System.Random(6875124);
    int randomNum;
    int count = 1;
    protected override void Use()
    {
        if (count >= 1)
        {
            count--;
            Debug.Log("Rare chest opened.");
            Vector3 spawnPos = transform.position;
            randomAmmoPack = ChooseRandomAmmoPack();
            randomAmmoPack.transform.position = spawnPos;
            base.Delete();
        }
    }

    public GameObject ChooseRandomAmmoPack()
    {
        int[] itemWeights = { 2, 2, 1, 2,3 };

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
            if (random <= total)
            {
                result = i;
                break;
            }
        }

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
