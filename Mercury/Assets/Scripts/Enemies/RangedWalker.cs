﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWalker : Enemy
{
    private float timer = 0;
    private Vector3 startPos;
    protected override void Start()
    {
        base.Start();
        CreateWeapon();
        health = 100;
        moveSpeed = 1f;
        startPos = transform.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Weapon position
        if (equippedWeapon)
        {
            equippedWeapon.transform.position = transform.position + equippedWeapon.transform.right * 0.5f - transform.up * 0.2f;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, 7.5f);

        PlayerActor closestPlayerActor = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerActor playerActor = colliders[i].GetComponent<PlayerActor>();

            // Is this collider a player
            if (playerActor != null)
            {
                if (closestPlayerActor == null)
                {
                    closestPlayerActor = playerActor;
                }
                // Is this player closer than the current closest player
                if (Vector3.Distance(playerActor.transform.position, transform.position) < Vector3.Distance(closestPlayerActor.transform.position, transform.position))
                {
                    closestPlayerActor = playerActor;
                }
            }

        }

        // If we found a player move towards it
        if (closestPlayerActor != null)
        {
            float playerRange = Vector3.Distance(closestPlayerActor.transform.position, transform.position);
            base.FaceDirection((closestPlayerActor.transform.position - transform.position).normalized);
            AimAtPlayer(closestPlayerActor.transform.position);

            if (playerRange < 5.5f) //checks if player is close enough to shoot.
            {
                AttackPlayer();
            }
            base.MoveForward();
        }
        else
        {
            timer += 1;
            if (timer > 4)
            {
                MoveRandomDir();
                timer = 0;
            }
            base.MoveForward();
        }

    }

    void MoveRandomDir()
    {
        base.FaceDirection(Quaternion.AngleAxis(Random.Range(-70.0f, 70.0f), Vector3.forward) * transform.position);
        equippedWeapon.transform.position = transform.position + equippedWeapon.transform.right * 0.5f - transform.up * 0.2f;
    }

    void CreateWeapon()
    {
        GameObject weapon = Factory.instance.CreatePistol();
        equippedWeapon = weapon.GetComponent<Weapon>();
        equippedWeapon.transform.position = transform.position;
        equippedWeapon.Equip();
    }

    public void AimAtPlayer(Vector3 direction)
    {
        if (equippedWeapon)
        {
            equippedWeapon.transform.right = (direction - transform.position).normalized;
        }
    }

    public void AttackPlayer()
    {
        if (equippedWeapon)
        {
            equippedWeapon.UseWeapon();
        }
    }
}
