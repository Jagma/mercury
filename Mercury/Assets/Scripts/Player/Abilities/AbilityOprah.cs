using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityOprah : Ability {

    float cooldownTime = 0;
    bool isProjectile = false;
    float ammoOffset = 0f;
    System.Random ran = new System.Random(91142069);
    List<string> spawnableItems;

    public override void Init() {
        base.Init();

        // Stats
        cooldown = 5f;
    }

    protected override void Use()
    {
        base.Use();
        GameObject freeItem = ChooseRandomItem();
        if (!isProjectile)
        {
            freeItem.transform.position = playerActor.transform.position +
             new Vector3(InputManager.instance.GetAimDirection(playerActor.model.playerID).x,
                0, InputManager.instance.GetAimDirection(playerActor.model.playerID).y);
        }
        else
        {
            FireProjectile(freeItem);
            isProjectile = false;
        }
    }
    

    public GameObject ChooseRandomItem()
    {
        spawnableItems = new List<string>();
        //ITEMS
        spawnableItems.Add("Enemy Walker");
        spawnableItems.Add("Wall");
        spawnableItems.Add("Rocket Launcher");
        spawnableItems.Add("Laser Rifle");
        spawnableItems.Add("Machine Gun");
        spawnableItems.Add("Pistol");
        spawnableItems.Add("Beam");
        spawnableItems.Add("Rocket");
        spawnableItems.Add("Bullet");
        spawnableItems.Add("Muzzle Flash");
        //spawnableItems.Add("Rocket Smoke Flash");
        spawnableItems.Add("Rocket Hit");
        spawnableItems.Add("Bullet Hit");
        spawnableItems.Add("Beam Hit");


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
                isProjectile = true;
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

    public void FireProjectile(GameObject aProjectile)
    {
        aProjectile.GetComponent<Projectile>().speed *= 2;
        aProjectile.transform.position = playerActor.transform.position;
        aProjectile.transform.right = new Vector3(InputManager.instance.GetAimDirection(playerActor.model.playerID).x, 0, InputManager.instance.GetAimDirection(playerActor.model.playerID).y);
        aProjectile.GetComponent<Projectile>().Update();
    }
}
