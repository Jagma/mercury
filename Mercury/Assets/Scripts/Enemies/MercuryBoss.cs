using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class
    MercuryBoss : RangedEnemies
{
    float stopDistance;
    float retreatDistance;
    float lineOfSight;
    protected override void Start()
    {
        base.Start();
        lineOfSight = 4f;
        health = 80;
        moveSpeed = 1f;
        equippedWeapon = Factory.instance.CreateLaserRayGun().GetComponent<Weapon>();
        equippedWeapon.SetWeaponDamage(1f);
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PlayerActor nearestPlayer = FindTarget();
        float distance = Vector3.Distance(nearestPlayer.transform.position, transform.position);
        if (nearestPlayer != null)
        {
            if (distance > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, nearestPlayer.transform.position, moveSpeed * Time.deltaTime);
            }
            if (distance < stopDistance && stopDistance > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            if (distance < retreatDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, nearestPlayer.transform.position, -moveSpeed * Time.deltaTime);
            }
        }
    }

    protected override void IdleMovement()
    {
        base.IdleMovement();
    }

    protected override void AimAtPlayer(Vector3 direction)
    {
        base.AimAtPlayer(direction);
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();
    }

    protected PlayerActor FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, lineOfSight);

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
        return closestPlayerActor;
    }

}

