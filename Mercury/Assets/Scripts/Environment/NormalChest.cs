using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChest : Chest
{
    int count = 1;

    protected override void Use()
    {
        if (count >= 1)
        {
            count--;

            GameObject[] chestObjects = GetRandomObjects();

            for (int i=0; i < chestObjects.Length; i ++) {
                chestObjects[i].transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
                chestObjects[i].GetComponent<Rigidbody>().AddForce(Vector3.up * 5 + Random.onUnitSphere * 2, ForceMode.Impulse);
            }
        }
    }

    protected virtual GameObject[] GetRandomObjects () {
        int objectCount = 1;

        if (Random.Range(0, 100) > 30) {
            objectCount += 1;
        } else
        if (Random.Range(0, 100) > 50) {
            objectCount += 2;
        } else
        if (Random.Range(0, 100) > 80) {
            objectCount += 3;
        }

        List<GameObject> objectList = new List<GameObject>();
        for (int i=0; i < objectCount; i ++) {
            objectList.Add(GetRandomItem());
        }

        return objectList.ToArray();
    }

    protected virtual GameObject GetRandomItem()
    {
        int[] itemWeights = {
            60, 30, 30, 30, 20, 30,
            50, 50, 50, 50, 50};

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
            if (random >= total) {
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

        // Consumables
        if (result == 7) {
            return Factory.instance.CreateMedkit();
        }
        if (result == 8) {
            return Factory.instance.CreateMedpack();
        }
        if (result == 9) {
            return Factory.instance.CreateBeamAmmoPack();
        }
        if (result == 10) {
            return Factory.instance.CreateBulletAmmoPack();
        }
        if (result == 11) {
            return Factory.instance.CreateRocketAmmoPack();
        }
        return null;
    }
}
