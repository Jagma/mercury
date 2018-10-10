﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAmmoPack : AmmoPack
{

    protected override void Start()
    {
        ammoAmount = 25;
        base.Start();
    }

        
protected override void Use(PlayerActor player)
    {
        base.Use(player);
        if (player.GetPlayerEquippedWeapon().GetComponent<Pistol>() != null || player.GetPlayerEquippedWeapon().GetComponent<MachineGun>() != null)
        {
            player.GetPlayerEquippedWeapon().SetAmmoCount(ammoAmount);
            base.Delete();
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}