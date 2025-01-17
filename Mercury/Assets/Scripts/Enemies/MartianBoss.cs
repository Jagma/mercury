﻿using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class 
    MartianBoss : Enemy
{
    private float cooldown;
    private float damage;
    float lineOfSight;
    float knockBack;
    float lastActivatedTime;
    bool isDashing = false;
    Rigidbody rigidbody;

    protected override void Start()
    {
        base.Start();
        health = 4000;
        moveSpeed = 2f;
        lineOfSight = 8f;
        cooldown = 2f;
        knockBack = 10f;
        lastActivatedTime = Time.time;
        damage = 20;

        rigidbody = GetComponent<Rigidbody>();
        Fightsounds();
        CreateArena();// This is to prevent boss from getting stuck and adding a boss fight feel.
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Time.time >= lastActivatedTime + cooldown && !isDashing)
        {
            isDashing = true;
            Dash();
            lastActivatedTime = Time.time;
        }
        if(Time.time >= lastActivatedTime + cooldown && isDashing)
        {
            isDashing = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerActor player = collision.collider.GetComponent<PlayerActor>();
        if(player != null && isDashing)
        {
            Vector3 expolsionPos = new Vector3(transform.position.x, 0f, transform.position.z);

            player.GetComponent<Rigidbody>().AddExplosionForce(knockBack, expolsionPos, 3f, 0.5f, ForceMode.Impulse);
            player.Damage(damage);
            lastActivatedTime = Time.time;
            isDashing = false;
            rigidbody.velocity = Vector3.zero;
        }
    }

    protected void Dash()
    {
        PlayerActor target = FindTarget();
        if (target != null)
        {
            Vector3 dir = GetDirection(target.transform.position);
            if (IsPossibleToHit(dir))
            {
                forwardDirection = target.transform.position - transform.position;

                gameObject.GetComponent<Rigidbody>().velocity = forwardDirection * moveSpeed;// Distance x movement speed
            }
        }
    }

    private void Fightsounds()
    {
        AudioManager.instance.StopAllAudio();
        AudioManager.instance.PlayAudio("Background_Mars_bossfight", 1, true);
        AudioManager.instance.PlayAudio("Boss_Mars_spawn", 1, false);
    }

    private void CreateArena()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, lineOfSight - 3f);
        foreach(Collider hit in hits)
        {
            Wall wall = hit.GetComponent<Wall>();
            if (wall != null)
            {
                wall.Damage(500);
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

    public Vector3 GetDirection(Vector3 playerPos)// Return direction between enemy and player
    {
        Vector3 heading = playerPos - transform.position;
        return heading / lineOfSight;
    }

    private bool IsPossibleToHit(Vector3 dir)
    {
        RaycastHit hit;

        int layerId = LayerMask.NameToLayer("Environment");
        int layerMask = 1 << layerId;
        //Raycasts to check if it is possible to hit a player
        if (Physics.Raycast(new Ray(transform.position, dir), out hit, lineOfSight, layerMask))
        {
            Collider collider = hit.collider;
            if (collider.GetComponent<Wall>() != null)
            {
                return false;
            }
            return true;
        }
        return true;
    }
}

