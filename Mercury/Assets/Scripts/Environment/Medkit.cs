using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : Health
{
    int count = 1;
    protected override void Start()
    {
        base.Start();
        healAmount = 50f;
    }

    protected override void Use(PlayerActor player)
    {
        if (count >= 1)
        {
            count--;
            player.health += healAmount;
            if (player.health > player.GetStartHealth())
                player.health = player.GetStartHealth();
            Debug.Log("Player healed with 50hp.");
            base.Delete();
        }
    }
}
