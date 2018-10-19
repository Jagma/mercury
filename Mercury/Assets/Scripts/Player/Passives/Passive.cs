using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Passive
{
    public PlayerActor player;

    public virtual void Init()
    {

    }
    public virtual void SingleAffect()
    {

    }
    public virtual void RecurringAffect()
    {

    }
}

public class PassiveRegen : Passive
{
    float hpRegen = 0.01f;

    public override void SingleAffect() {}

    public override void RecurringAffect()
    {
        if (player.model.playerActive == true)
        {
            player.HealPlayer(hpRegen);
        }
    }
}

public class PassiveIncreasedMaxHP : Passive
{
    float hpIncrease = 50f;

    public override void SingleAffect()
    {
        player.model.maxHealth = hpIncrease;
    }
    public override void RecurringAffect() {}

}

public class PassiveMovementSpeed : Passive
{
    float movementSpeedIncrease = 0.5f;
    public override void SingleAffect()
    {
        player.model.moveMaxSpeed += 0.5f;
    }
    public override void RecurringAffect() { }
}

public class PassiveMaxAmmoIncrease : Passive
{
    float AmmoIncrease = 50f;

    public override void SingleAffect()
    {
        //player.model.equippedWeapon.ammoMaxInventory *= 2;
        //player.model.secondaryWeapon.ammoMaxInventory *= 2;
    }
    public override void RecurringAffect() { }
}
