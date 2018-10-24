using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class MalfeasanceBoss : Enemy
{
    private float cooldown;
    private float damage;
    float lineOfSight;
    float lastActivatedTime;
    bool isShootingSkulls;
    bool isShootingLazer;
    float shotAngle;


    private int shotsCount;
    private float shotSpeed;

    float retreatDistance;
    float stopDistance;


    Rigidbody rigidbody;

    protected override void Start()
    {
        base.Start();
        health = 1000;
        moveSpeed = 2f;

        shotSpeed = 2;
        shotsCount = 10;
        shotAngle = 0;
        retreatDistance = 0.5f;
        stopDistance = 2f;
        lineOfSight = 10f;
        damage = 10;
        cooldown = 2f;
        lastActivatedTime = Time.time;

        isShootingSkulls = true;
        isShootingLazer = false;

        rigidbody = GetComponent<Rigidbody>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PlayerActor nearestPlayer = FindTarget();
        if(isShootingSkulls)
        {
            if(Time.time >= lastActivatedTime + cooldown)
            {
                Shoot(shotAngle);
                lastActivatedTime = Time.time;
                shotAngle += 45;
            }
        }
        float distance = Vector3.Distance(nearestPlayer.transform.position, transform.position);
        if (nearestPlayer != null)
        {
            if (distance > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position,nearestPlayer.transform.position, moveSpeed * Time.deltaTime);
            }
            if(distance < stopDistance && stopDistance > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            if(distance < retreatDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, nearestPlayer.transform.position, -moveSpeed * Time.deltaTime);
            }
        }
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

    private void Shoot(float angle)
    {
        cooldown = 1f;

        shotSpeed += 0.2f;
        for(int i = 0; i < shotsCount; i++)// Places skulls around him and in predefined direction.
        {
            Vector3 pos = new Vector3
            (
                x: transform.position.x + 1f * Mathf.Sin(angle * Mathf.Deg2Rad),
                y: 1f,
                z: transform.position.z + 1f * Mathf.Cos(angle * Mathf.Deg2Rad)
            );

            GameObject skull = Factory.instance.CreateSkulls();
            skull.transform.position = pos;
            skull.transform.right = GetDirection(pos);
            skull.GetComponent<Skull>().speed = shotSpeed;

            angle += 360 / shotsCount; // Spiral effect
        }
    }

    public Vector3 GetDirection(Vector3 pos)// Return direction between enemy and player
    {
        Vector3 heading = pos - new Vector3(transform.position.x, 1f, transform.position.z);
        return heading / 1f;
    }


}

