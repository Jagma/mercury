using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityOprah : Ability {

    bool isProjectile = false;
    bool isLazer = false;
    float count = 0;
    GameObject freeItem;
    System.Random ran = new System.Random(91142069);
    List<string> spawnableItems;

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

        if (!isProjectile && !isLazer)
        {
            freeItem.transform.position = playerActor.transform.position + new Vector3(aimDirection.x, 0, aimDirection.y) * 1f;

        }
        else if (isLazer)
        {
            // Visual look at camera
            freeItem.transform.eulerAngles = new Vector3(45, 45, -freeItem.transform.eulerAngles.y + 45);
            freeItem.transform.position = playerActor.transform.position + new Vector3(aimDirection.x, aimDirection.y, aimDirection.z) * 1f; 

            //Beam(); - crashes unity.
            GameObject.Destroy(freeItem, 1f);
            isLazer = false;
            
        }
        else
        {
            freeItem.GetComponent<Projectile>().speed *= 2;
            freeItem.transform.position = playerActor.transform.position;
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
        spawnableItems = new List<string>();
        //ITEMS
        //spawnableItems.Add("Enemy Walker");
        //spawnableItems.Add("Wall");
        //spawnableItems.Add("Rocket Launcher");
        //spawnableItems.Add("Laser Rifle");
        //spawnableItems.Add("Machine Gun");
        //spawnableItems.Add("Pistol");
        spawnableItems.Add("Beam");
        //spawnableItems.Add("Rocket");
        //spawnableItems.Add("Bullet");
        //spawnableItems.Add("Muzzle Flash");
        //spawnableItems.Add("Rocket Smoke Flash");
        //spawnableItems.Add("Rocket Hit");
        //spawnableItems.Add("Bullet Hit");
        //spawnableItems.Add("Beam Hit");


        int randomNum = ran.Next(0, spawnableItems.Count);
        Debug.Log("Count: " + spawnableItems.Count +"\n Random Number: "+ randomNum);
        switch (spawnableItems[randomNum])
        {
            case "Enemy Walker":
                return Factory.instance.CreateEnemyWalker();
            case "Wall":
                return Factory.instance.CreateWall();
            case "Rocket Launcher":
                return Factory.instance.CreateRocketLauncher();
            case "Laser Rifle":
                return Factory.instance.CreateLaserRifle();
            case "Machine Gun":
                return Factory.instance.CreateMachineGun();
            case "Pistol":
                return Factory.instance.CreatePistol();
            case "Beam":
                isLazer = true;
                return Factory.instance.CreateBeamNeon();
            case "Rocket":
                isProjectile = true;
                return Factory.instance.CreateRocket();
            case "Bullet":
                isProjectile = true;
                return Factory.instance.CreateBullet();
            case "Muzzle Flash":
                return Factory.instance.CreateMuzzleFlash();
            case "Rocket Smoke Flash":
                return Factory.instance.CreateRocketSmokeFlash();
            case "Rocket Hit":
                return Factory.instance.CreateRocketHit();
            case "Bullet Hit":
                return Factory.instance.CreateBulletHit();
            case "Beam Hit":
                return Factory.instance.CreateBeamHit();
            default:
                return null;
        }

    }

}
