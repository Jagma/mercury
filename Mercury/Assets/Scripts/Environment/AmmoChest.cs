using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoChest : Chest
{
    GameObject randomAmmoPack;
    System.Random ran = new System.Random(91142069);
    int randomNum;
    int count = 1;
    protected override void Use()
    {
        if (count >= 1)
        {
            count--;
            Debug.Log("Ammo chest opened.");
            Vector3 spawnPos = transform.position;
            randomAmmoPack = ChooseRandomAmmoPack();
            randomAmmoPack.transform.position = spawnPos;
            base.Delete();
        }
    }

    public GameObject ChooseRandomAmmoPack()
    {
        randomNum = ran.Next(0, 100);
        if (randomNum >= 0 && randomNum < 11)//10% Laser Rifle ammo
        {
            return Factory.instance.CreateBeamAmmoPack();
        }
        else if (randomNum > 20 && randomNum < 61) //40% rocket launcher ammo
        {
            return Factory.instance.CreateBulletAmmoPack();
        }
        else
        {
            return Factory.instance.CreateRocketAmmoPack();
        }
    }

}
