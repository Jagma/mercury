using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability {

    public PlayerActor playerActor;
    float lastUseTime = 0f;
    public float cooldown = 1f;

    public virtual void Init () {

    }

    
    public void UseAbility () {
        if (Time.time <= lastUseTime + cooldown) {
            return;
        }
        else {
            lastUseTime = Time.time;
            Use();
        }
    }

    protected virtual void Use() {
    }    
}
