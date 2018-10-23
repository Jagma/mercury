using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MeleeEnemies
{

    protected override void Start()
    {
        base.Start();

        health = 25;
        moveSpeed = 2f;
    }

    protected override void CreateWeapon()
    {
        base.CreateWeapon();
        GameObject weapon;
        weapon = Factory.instance.CreateSword();
        equippedWeapon = weapon.GetComponent<Weapon>();
        equippedWeapon.transform.position = transform.position;
        equippedWeapon.Equip();
        equippedWeapon.equipped = true;
        equippedWeapon.SetWeaponDamage(1.25f);
    }

    protected override void AimAtPlayer(Vector3 direction)
    {
        base.AimAtPlayer(direction);
    }

    protected override void FixedUpdate ()
    {
        base.FixedUpdate();
    }
}
