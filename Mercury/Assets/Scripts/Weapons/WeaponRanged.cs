using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRanged : Weapon
{
    protected float ammoOffset = 0.2f;
    protected float ammoRandomness = 1f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Use()
    {
        base.Use();
        if (cooldownRemaining > 0)
        {
            return;
        }
        else
        {
            if (ammoCount > 0) //if there is still ammo left in the mag.
            {
                cooldownRemaining = cooldown;
                ammoCount--;
            }
            else if (ammoInventory > 0) //if there is still ammo in the inventory.
            {
                ReloadWeapon();
            }
            else //if the weapon is completely out of ammo.
            {
                return;
                //play sound for out of ammo..
            }
        }
    }

    public void SetAmmoCount(int ammoValue)
    {
        if ((ammoInventory + ammoValue) > ammoMaxInventory) //checks to see if the current ammo inventory and the newly ammoun count obtained is greater than the max ammo allowed.
        {
            ammoInventory = ammoMaxInventory;
            ReloadWeapon();
        }
        else
        {
            ammoInventory += ammoValue;
            ReloadWeapon();
        }
    }

    protected override void Update ()
    {
        base.Update();
    }

    private void ReloadWeapon()
    {
        if (ammoCount < ammoMax) //checks to see if there is less bullets in the mag of the weapon than the max it can contain.
        {
            if (ammoInventory <= ammoMax) //checks to see if the amount of ammo in the inventory is smaller or equal to the maximum amount of ammo the weapon can take in a mag.
            {
                if (ammoCount == 0)  //checks to see if the weapon's mag is empty.
                {
                    ammoCount = ammoInventory;
                    ammoInventory = 0;
                }
                else //if weapon mag is not empty.
                {
                    int temp = ammoMax - ammoCount; //amount of bullets not in weapon mag.
                    if (ammoInventory < temp) //if there is less ammo left in inventory than needed.
                    {
                        ammoCount += ammoInventory;
                        ammoInventory = 0;
                    }
                    else
                    {
                        ammoInventory -= temp;
                        ammoCount += temp;
                    }
                }
            }
            else if (ammoInventory > ammoMax) //checks to see if the amount of ammo in the inventory is more than the maximum amount of ammo the weapon can take in a mag.
            {
                if (ammoCount == 0) //checks to see if the weapon's mag is empty.
                {
                    ammoCount = ammoMax;
                    ammoInventory -= ammoMax;
                }
                else //if weapon mag is not empty.
                {
                    int temp = ammoMax - ammoCount; //amount of bullets not in weapon mag.
                    ammoInventory -= temp;
                    ammoCount += temp;
                }
            }
        }
    }
}