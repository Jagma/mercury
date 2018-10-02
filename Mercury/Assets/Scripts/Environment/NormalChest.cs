using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChest : Chest
{
    GameObject freeItem;
    System.Random ran = new System.Random(91142069);
    List<string> spawnableItems;


    public override void Init()
    {
        base.Init();
    }

    protected override void Use()
    {
        freeItem = ChooseRandomItem();
        Vector3 spawnPos = transform.position;
        freeItem.transform.position = spawnPos;
        base.Use();
    }

    public GameObject ChooseRandomItem()
    {
        spawnableItems = new List<string>();

        spawnableItems.Add("Rocket Launcher");
        spawnableItems.Add("Laser Rifle");
        spawnableItems.Add("Machine Gun");
        spawnableItems.Add("Pistol");

        int randomNum = ran.Next(0, spawnableItems.Count);

        switch (spawnableItems[randomNum])
        {
            case "Rocket Launcher":
                return Factory.instance.CreateRocketLauncher();
            case "Laser Rifle":
                return Factory.instance.CreateLaserRifle();
            case "Machine Gun":
                return Factory.instance.CreateMachineGun();
            case "Pistol":
                return Factory.instance.CreatePistol();
            default:
                return null;
        }

    }

}
