using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float cooldown = 0.1f;
    public bool equipped = false;

    public int ammoMaxInventory = 120; //ammoMMaxInventory is the maximum ammount of ammo a weapon can have in the player's inventory.
    public int ammoInventory = 120; //ammoInventory is the amount of ammo for the weapon in the player's inventory.
    public int ammoCount = 120; //ammoCount is the amount of ammo currently in the weapon's mag.
    public int ammoMax = 12; //ammoMax is the amount of ammo the weapon is allowed to have in one mag.

    protected Transform visual;
    protected float cooldownRemaining = 0f;
    protected float damage = 0;

    private void Awake()
    {
        visual = transform.Find("Visual");
        InitWeaponStats();
    }

    protected virtual void InitWeaponStats() {

    }

    protected virtual void Start()
    {

    }

    public void Equip()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<SphereCollider>().enabled = false;
        equipped = true;
    }

    public void Dequip()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<SphereCollider>().enabled = true;
        equipped = false;
    }


    public void UseWeapon()
    {
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
                GameProgressionManager.instance.IncreaseBulletAmountUsed();
                Use();
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

    public void SetWeaponDamage (float value)
    {
        damage = value;
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
    public void SetMaxAmmoCount(int maxAmmoIncrease)
    {
        ammoMaxInventory += maxAmmoIncrease;
        SetAmmoCount(ammoMaxInventory);// Adjusts the ammo count for next level
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
    protected virtual void Use()
    {

    }

    protected virtual void Update()
    {
        // Cooldown
        if (cooldownRemaining >= 0)
        {
            cooldownRemaining -= Time.deltaTime;
        }


        // Visual look at camera
        visual.eulerAngles = new Vector3(45, 45, -transform.eulerAngles.y + 45);

        Vector2 norm = Quaternion.AngleAxis(-45, Vector3.up) * transform.right;
        if (norm.x < 0)
        {
            Vector3 x = Quaternion.AngleAxis(180, visual.right) * visual.forward;
            visual.forward = x;
            visual.eulerAngles = new Vector3(visual.eulerAngles.x, visual.eulerAngles.y, 180+ transform.eulerAngles.y -45);
        }
    }
}
