using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityOprah : Ability
{

    bool isProjectile = false;
    float count = 0;
    GameObject freeItem;
    System.Random ran = new System.Random(91142069);
    int randomNum;

    public override void Init()
    {
        base.Init();

        // Stats
        cooldown = 0f;
    }

    protected override void Use()
    {
        base.Use();
        freeItem = ChooseRandomItem();
        Vector2 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        Vector3 normalizedAim = Quaternion.AngleAxis(45, Vector3.up)* new Vector3(aimDirection.x,0,aimDirection.y);
        Vector3 position = playerActor.transform.position + new Vector3(normalizedAim.x, normalizedAim.y, normalizedAim.z) * 1f;
        Debug.Log("OPRAH Spawned: " + freeItem.gameObject.name);
        if (!isProjectile)
        {

            freeItem.transform.position = position;
            freeItem.transform.right = aimDirection;

        }
        else
        {
            //freeItem.GetComponent<Projectile>().speed *= 2;
            freeItem.transform.position = position;
            freeItem.transform.right = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(aimDirection.x, 0, aimDirection.y);
            //freeItem.transform.eulerAngles = new Vector3(0, 0, 0);
            freeItem.GetComponent<Projectile>().Update();
            isProjectile = false;
        }
    }

    private GameObject ChooseRandomItem()
    {

        int[] categoryWeight = { 5, 5, 30, 60 };// HIT EFFECTS| ENEIES| ROUNDS| WEAPONS
        int[] weaponWeights = {25,30,30,25};// MACHINE GUN| LAZER RIFLE| ROCKET LAUNCHER| PISTOL
        int[] enemyWeights = {60, 40};// MELEE| RANGED

        int result = GetResult(categoryWeight);
        Debug.Log("CATEGORY " + result);
        if(result == 0)//Hit effects
        {
            int roll = ran.Next(0, 4);

            if (roll == 0)
                return Factory.instance.CreateBeamHit();
            if (roll == 1)
                return Factory.instance.CreateBulletHit();
            if (roll == 2)
                return Factory.instance.CreateRocketHit();
            if (roll == 3)
                return Factory.instance.CreateRocketSmokeFlash();
            if (roll == 4)
                return Factory.instance.CreateMuzzleFlash();
        }
        if (result == 1)//Enemies
        {
            result = GetResult(enemyWeights);
            Debug.Log("ENEMY " + result);
            if (result == 0)
                return Factory.instance.CreateEnemyWalker();
            if (result == 1)
                return Factory.instance.CreateRangedWalker();
        }
        if (result == 2)//Rounds
        {
            int roll = ran.Next(0, 2);
            isProjectile = true;

            if (roll == 0)
                return Factory.instance.CreateBullet();
            if (roll == 1)
                return Factory.instance.CreateRocket();
        }
        if (result == 3)//weapons
        {
            result = GetResult(weaponWeights);
            Debug.Log("WEAPON " + result);
            if (result == 0)
                return Factory.instance.CreateMachineGun();
            if (result == 1)
                return Factory.instance.CreateLaserRifle();
            if (result == 2)
                return Factory.instance.CreateRocketLauncher();
            if (result == 3)
                return Factory.instance.CreatePistol();
        }
        return null;
    }

    private int GetResult(int[] array)
    {
        int total = 0;
        Array.ForEach(array, delegate (int i) { total += i; });
        randomNum = ran.Next(0, total);

        total = 0;
        int result = 0;
        for (int i = 0; i < array.Length; i++)
        {
            total += array[i];
            if (randomNum <= total)
            {
                result = i;
                break;
            }
        }
        return result;
    }

}
