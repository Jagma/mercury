using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemies : Enemy
{

    protected override void Start()
    {
        base.Start();
        health = 25;
        moveSpeed = 2f;
    }

    Collider[] colliders;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Weapon position
        if (equippedWeapon)
        {
            equippedWeapon.transform.position = transform.position + equippedWeapon.transform.right * 0.5f - transform.up * 0.2f;
        }

        int layerId = LayerMask.NameToLayer("Player");
        int layerMask = 1 << layerId;
        colliders = Physics.OverlapSphere(transform.position, 7.5f, layerMask);

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
                if (Vector3.Distance(playerActor.transform.position, transform.position) <
                    Vector3.Distance(closestPlayerActor.transform.position, transform.position))
                {
                    closestPlayerActor = playerActor;
                }
            }

        }

        // If we found a player move towards it
        if (closestPlayerActor != null && closestPlayerActor.model.playerActive)
        {
            base.FaceDirection((closestPlayerActor.transform.position - transform.position).normalized);
            AimAtPlayer(closestPlayerActor.transform.position);
            base.MoveForward();
            AttackPlayer(closestPlayerActor.transform.position);
        }


    }

    protected virtual void AimAtPlayer(Vector3 direction)
    {
        if (equippedWeapon)
        {
            equippedWeapon.transform.right = (direction - transform.position).normalized;
        }
    }

    protected virtual void AttackPlayer(Vector3 direction)
    {
        if (equippedWeapon)
        {
            if (Vector3.Distance(direction, transform.position) < 2f)
                equippedWeapon.UseWeapon();
        }
    }
}
