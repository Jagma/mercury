using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability {

    public PlayerActor playerActor;

    //CoolDowns
    private float cooldownTime;
    private bool onCooldown = false;

    public virtual void Use() {

    }

    public void SetCooldownTime(float aSeconds)//Add CoolDownTime to ability
    {
        this.cooldownTime = aSeconds;
        onCooldown = true;
    }
    public void Update()
    {
        if(cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
        }
        if(cooldownTime <= 0)
        {
            onCooldown = false;
        }
    }

    public float CoolDownTimeLeft()
    {
        return cooldownTime;
    }

    public bool IsOnCooldown()
    {
        return onCooldown;
    }
}
