using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockettAmmoPack : AmmoPack
{

    protected override void Start()
    {
        ammoAmount = 1;
        base.Start();
    }

    protected override void Use(PlayerActor player)
    {
        base.Use(player);
        if (player.GetPlayerEquippedWeapon().GetComponent<RocketLauncher>() != null)
        {
            player.GetPlayerEquippedWeapon().GetComponent<WeaponRanged>().SetAmmoCount(ammoAmount);
            base.Delete();
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}