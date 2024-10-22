﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAmmoPack : AmmoPack
{

    protected override void Start()
    {
        ammoAmount = 50;
        base.Start();
    }

    protected override void Use(PlayerActor player)
    {
        base.Use(player);
        if (player.GetPlayerEquippedWeapon().GetComponent<LaserRayGun>() != null ||
            player.GetPlayerEquippedWeapon().GetComponent<LaserPistol>() != null || 
            player.GetPlayerEquippedWeapon().GetComponent<LaserMachineGun>() != null)
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