
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityOprah : Ability
{

    bool isProjectile = false;
    GameObject freeItem;
    int randomNum;

    public override void Init()
    {
        base.Init();

        // Stats
        cooldown = 4f;
    }

    protected override void Use()
    {
        base.Use();

        freeItem = ChooseRandomItem();

        //Get aim direction and rotate by 45 degrees
        Vector2 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        if (aimDirection.magnitude <= 0.1)
        {
            aimDirection = aimDirection * 100f;
        }
        Vector3 normalizedAim = Quaternion.AngleAxis(45, Vector3.up)* new Vector3(aimDirection.x,0,aimDirection.y);

        //Position to spawn item
        Vector3 position = playerActor.transform.position + new Vector3(normalizedAim.x, normalizedAim.y, normalizedAim.z) * placementOffset;

        if (!isProjectile)//for everything that is not a projectile
        {

            freeItem.transform.position = position;
            freeItem.transform.right = aimDirection;
        }
        else
        {
            freeItem.transform.position = position;
            freeItem.transform.right = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(aimDirection.x, 0, aimDirection.y);
            freeItem.GetComponent<Projectile>().Update();
            isProjectile = false;
        }
        AudioManager.instance.PlayAudio("Oprah You Get A Car", 1, false);
    }

    private GameObject ChooseRandomItem()
    {

        int[] categoryWeight = 
            {
                2, // Hit Effect
                2, // Enemies
                5, // Rounds
                5, // Weapons
                1, // Chest
                2, // Medkit
                2  //Ammo Pack
            };

        int[] weaponWeights = 
            {
                2, // Machine Gun
                1, // Lazer Pistol
                2, // Lazer Rifle
                3, // Rocket Launcher
                1, // Pistol
                2, // Shotgun
                1, // Flamethrower
            };
        int[] enemyWeights = {60, 40};

        int result = GetResult(categoryWeight);//Choose category

        #region Hit Effects
        if(result == 0)//Hit effects
        {
            int roll = UnityEngine.Random.Range(0, 4);

            //Hit effects roll for random
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
        #endregion
        #region Enemies
        if (result == 1)
        {
            result = GetResult(enemyWeights);//Use weigths to balance enemy spawn chance

            if (result == 0)
                return Factory.instance.CreateEnemyWalker();
            if (result == 1)
                return Factory.instance.CreateRangedWalker();
        }
        #endregion
        #region Rounds
        if (result == 2)
        {
            int roll = Random.Range(0, 2);//roll for random
            isProjectile = true;

            if (roll == 0)
                return Factory.instance.CreateBullet();
            if (roll == 1)
                return Factory.instance.CreateRocket();
        }
        #endregion
        #region Weapons
        if (result == 3)
        {
            result = GetResult(weaponWeights);//Use weigths to balance weapon spawn chance

            if (result == 0)
                return Factory.instance.CreateMachineGun();
            if (result == 1)
                return Factory.instance.CreateLaserRifle();
            if (result == 2)
                return Factory.instance.CreateRocketLauncher();
            if (result == 3)
                return Factory.instance.CreatePistol();
        }
        #endregion
        #region Chest
        if (result == 4)
        {
            int roll = Random.Range(0, 2);

            if (roll == 0)
                return Factory.instance.CreateNormalChest();
            if (roll == 1)
                return Factory.instance.CreateRareChest();

        }
        #endregion
        #region Medkit
        if(result == 5)
        {
            int roll = Random.Range(0, 2);

            if (roll == 0)
                return Factory.instance.CreateMedkit();
            if (roll == 1)
                return Factory.instance.CreateMedpack();
        }
#endregion
        #region Ammo Pack
        if(result == 6)
        {
            int roll = Random.Range(0, 3);

            if (roll == 0)
                return Factory.instance.CreateRocketAmmoPack();
            if (roll == 1)
                return Factory.instance.CreateBeamAmmoPack();
            if (roll == 2)
                return Factory.instance.CreateBulletAmmoPack();
        }
#endregion
        return null;
    }

    private int GetResult(int[] weights)
    {
        int total = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            total += weights[i];
        }

        randomNum = Random.Range(0, total+1);

        total = 0;
        int result = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            total += weights[i];
            if (randomNum <= total)
            {
                result = i;
                break;
            }
        }
        return result;
    }

}
