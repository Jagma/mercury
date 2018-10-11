using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordWalker : Enemy
{
    protected override void Start()
    {
        base.Start();
        CreateWeapon();
        health = 100;
        moveSpeed = 1.25f;

        movementTime = Random.Range(2f, 4f);
        waitTime = Random.Range(0.25f, 2f);
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

        colliders = Physics.OverlapSphere(transform.position, 7.5f);

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
            IdleMovement();
        }

    }

    float timer = 0;
    float movementTime = 3;
    float waitTime = 1;
    void IdleMovement()
    {
        timer += Time.deltaTime;
        if (timer > movementTime + waitTime)
        {


            Vector3 direction = Random.onUnitSphere;
            direction.y = 0;
            direction.Normalize();
            FaceDirection(direction);

            movementTime = Random.Range(2.3f, 3.7f);
            waitTime = Random.Range(0.4f, 1.8f);

            timer = 0;
        }
        if (timer < movementTime)
        {
            base.MoveForward();
        }
    }

    void CreateWeapon()
    {
        GameObject weapon;
        weapon = Factory.instance.CreateMachineGun();
        equippedWeapon = weapon.GetComponent<Weapon>();
        equippedWeapon.transform.position = transform.position;
        equippedWeapon.Equip();
        equippedWeapon.equipped = true;
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
