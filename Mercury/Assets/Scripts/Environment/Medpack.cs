﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medpack : Health
{
    int count = 1;
    protected override void Start()
    {
        base.Start();
        healAmount = 25f;
    }

    protected override void Use(PlayerActor player)
    {
        if (count >= 1)
        {
            count--;
            player.health += healAmount;
            Debug.Log("Player healed with 25hp.");
            base.Delete();
        }
    }
}