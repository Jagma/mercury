using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityOprah : Ability {

    bool isProjectile = false;
    bool isLazer = false;
    float count = 0;
    GameObject freeItem;
    System.Random ran = new System.Random(91142069);
    int randomNum;

    public override void Init() {
        base.Init();

        // Stats
        cooldown = 0f;
    }

    protected override void Use()
    {
        base.Use();
        freeItem = ChooseRandomItem();
        Vector3 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);

        Debug.Log("OPRAH Spawned: " + freeItem.gameObject.name);
        if (!isProjectile && !isLazer)
        {
            freeItem.transform.position = playerActor.transform.position + new Vector3(aimDirection.x, 0, aimDirection.y) * 2f;

        }
        else if (isLazer)
        {
            // Visual look at camera
            freeItem.transform.eulerAngles = new Vector3(45, 45, -freeItem.transform.eulerAngles.y + 45);
            freeItem.transform.position = playerActor.transform.position + new Vector3(aimDirection.x, aimDirection.y, aimDirection.z) * 1f; 

            Beam(); //- crashes unity.
            GameObject.Destroy(freeItem, 1f);
            isLazer = false;
            
        }
        else
        {
            freeItem.GetComponent<Projectile>().speed *= 2;
            freeItem.transform.position = playerActor.transform.position + aimDirection *1f;
            freeItem.transform.right = new Vector3(aimDirection.x, 0 , aimDirection.y);
            freeItem.GetComponent<Projectile>().Update();
            isProjectile = false;
        }
    }

    void Beam()
    {
        count += Time.deltaTime;
        while (count < 4)
        {
            Vector3 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);
            freeItem.transform.position = playerActor.transform.position;
            freeItem.transform.right = new Vector3(aimDirection.x, 0, aimDirection.y);
            freeItem.GetComponent<Beam>().UpdateVisual(freeItem.transform.position, aimDirection);
        }
    }
    public GameObject ChooseRandomItem()
    {
        randomNum = ran.Next(0, 100);
        Debug.Log("RANDOM ITEM ROLL" + randomNum);
        if(randomNum >= 0 && randomNum < 11)//10% FLASHES
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
        if (randomNum > 10 && randomNum < 21)//10% ENEMIES
        {
            int roll = ran.Next(0, 1);

            if (roll == 0)
                return Factory.instance.CreateEnemyWalker();
            if (roll == 1)
                return Factory.instance.CreateRangedWalker();
        }
        if(randomNum > 20 && randomNum < 61 )//40% BULLETS
        {
            int roll = ran.Next(0, 2);
            isProjectile = true;

            if (roll == 0)
                return Factory.instance.CreateBullet();
            if (roll == 1)
                return Factory.instance.CreateRocket();
            if (roll == 2)
            {
                isProjectile = false;
                isLazer = true;
                return Factory.instance.CreateBeamNeon();
            }
                
        }
        if(randomNum > 60 && randomNum < 101)//40% WEAPONS
        {
            int roll = ran.Next(0, 3);

            if (roll == 0)
                return Factory.instance.CreateMachineGun();
            if (roll == 1)
                return Factory.instance.CreateLaserRifle();
            if (roll == 2)
                return Factory.instance.CreateRocketLauncher();
            if (roll == 3)
                return Factory.instance.CreatePistol();
        }
        return null;
    }

}
