using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWalker : Enemy
{
    private Weapon equippedWeapon;

    protected override void Start()
    {
        base.Start();

        health = 5000;
        moveSpeed = 2f;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        // Weapon position
        if (equippedWeapon) {
            equippedWeapon.transform.position = transform.position + equippedWeapon.transform.right * 0.5f - transform.up * 0.2f;
        }

        // TODO: Create ranged enemy behaviour here
        // If I dont have a weapon run forward, every 3-5 seconds change direction
        // If I have a weapon check if a player is in range.
        // If a player is in range aim towards the player and attack
    }
}
