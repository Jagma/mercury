using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Enemy
{

    protected override void Start()
    {
        base.Start();

        health = 50;
        moveSpeed = 2f;
    }

    protected override void FixedUpdate ()
    {
        base.FixedUpdate();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 7.5f);

        PlayerActor closestPlayerActor = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerActor playerActor = colliders[i].GetComponent<PlayerActor>();

            // Is this collider a player
            if (playerActor != null) {
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
        if (closestPlayerActor != null)
        {
            base.FaceDirection((closestPlayerActor.transform.position - transform.position).normalized);
            base.MoveForward();
        }

    }
}
